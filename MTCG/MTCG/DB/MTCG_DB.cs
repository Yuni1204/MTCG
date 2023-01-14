using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using MTCG.App;
using MTCG.json;
using static System.Net.WebRequestMethods;
using System.Data.SqlClient;
using System.Text;
using MTCG.Server;
using Newtonsoft.Json;

namespace MTCG.DB
{
    public class DataBase
    {
        // Obtain connection string information from the portal
        //
        private static string Host = "localhost";
        private static string User = "postgres";
        private static string DBname = "MTCG_DB";
        private static string Password = "12345678";
        private static string Port = "5432";

        private string connString;
        private string SQLstatement = null;

        public DataBase()
        {
            // Build connection string using parameters from portal
            connString = $"Server={Host};Username={User};Database={DBname};Port={Port};Password={Password};SSLMode=Prefer;";
        }

        public bool ResetTables()
        {
            List<string> tables = new List<string>();
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                SQLstatement = new SQL_Statements().allTables();

                using (var command = new NpgsqlCommand(SQLstatement, conn))
                {
                    var res = command.ExecuteReader();
                    if (res.HasRows)
                    {
                        while (res.Read())
                        {
                            //Console.WriteLine(res.GetValue(0));
                            //string test = ;
                            tables.Add(res.GetValue(0).ToString());
                        }
                    }
                    res.Close();
                }

                foreach (var table in tables)
                {
                    using (var command = new NpgsqlCommand(new SQL_Statements().truncateTable(table), conn))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"Table '{table}' reset!");
                    }
                }
                conn.Close();
                return true;
            }
        }

        public string addUser(UsersTable userdata)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                SQLstatement = "CREATE TABLE IF NOT EXISTS users" +
                               "(username VARCHAR(50) PRIMARY KEY, password VARCHAR(50), name VARCHAR(50), bio VARCHAR(50), " +
                               "image VARCHAR(10), coins integer DEFAULT 20, elo integer DEFAULT 100, games integer DEFAULT 0, " +
                               "wins integer DEFAULT 0, losses integer DEFAULT 0, draws integer DEFAULT 0)";
                using (var command = new NpgsqlCommand(SQLstatement, conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Finished creating table 'User' if not exists");
                }

                SQLstatement = new SQL_Statements().createUser();
                //string values = new SQL_Statements().connectValuesToOneString(new string[] { userdata.Username, userdata.Password });

                try
                {
                    using (var command = new NpgsqlCommand(SQLstatement, conn))
                    {
                        command.Parameters.AddWithValue("@username", userdata.Username);
                        command.Parameters.AddWithValue("@password", userdata.Password);
                        command.ExecuteNonQuery();
                        Console.Out.WriteLine("Added "+userdata.Username+" into DB!");
                    }
                }
                catch (Npgsql.PostgresException ex)
                {
                    if (ex.SqlState == "23505")
                    {
                        return new Server.HttpResponse().UserCreate409();
                    }
                    Console.WriteLine("some other Exception in PostgresExc");
                    Console.WriteLine(ex.Message);
                }

                conn.Close();
            }

            return new Server.HttpResponse().UserCreate201();
            //return new Server.HttpResponse().testmethod();
            //"HTTP / 1.1 200 OK \r"
        }

        public string searchUser(UsersTable userdata)//session
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                SQLstatement = new SQL_Statements().searchUser();
                try
                {
                    using (var command = new NpgsqlCommand(SQLstatement, conn))
                    {
                        command.Parameters.AddWithValue("@username", userdata.Username);
                        command.Parameters.AddWithValue("@password", userdata.Password);
                        var result = command.ExecuteReader();
                        if (result.HasRows)
                        {
                            return new Server.HttpResponse().Session200(userdata.Username);
                        }
                        else
                        {
                            return new Server.HttpResponse().Session401();
                        }
                        
                    }
                }
                catch (Npgsql.PostgresException ex) // das ist von addUser(), sollte überarbeitet werden
                {
                    //if (ex.SqlState == "23505")
                    //{
                    //    return new Server.HttpResponse().UserCreate409();
                    //}
                    Console.WriteLine(ex.Message);
                }

                conn.Close();
            }

            return null;
        }

        public string addPackage(List<CardsJson> package)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                //create Table "packages"
                using (var command = new NpgsqlCommand(new SQL_Statements().createTablePackages(), conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Finished creating table 'packages' if not exists");
                }

                //create Table "cards"
                using (var command = new NpgsqlCommand(new SQL_Statements().createTableCards(), conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Finished creating table 'cards' if not exists");
                }

                //check if a card_id already exists in database
                using (var command = new NpgsqlCommand(new SQL_Statements().checkForDuplicateCards(), conn))
                {
                    command.Parameters.AddWithValue("@id1", package.ElementAt(0).Id);
                    command.Parameters.AddWithValue("@id2", package.ElementAt(1).Id);
                    command.Parameters.AddWithValue("@id3", package.ElementAt(2).Id);
                    command.Parameters.AddWithValue("@id4", package.ElementAt(3).Id);
                    command.Parameters.AddWithValue("@id5", package.ElementAt(4).Id);
                    var result = command.ExecuteReader();
                    if (result.HasRows) //means there is at least one duplicate
                    {
                        return new Server.HttpResponse().Package409();
                    }
                    Console.Out.WriteLine("no Card duplicates found");
                    result.Close();
                }

                //create package
                using (var command = new NpgsqlCommand(new SQL_Statements().addPackage(), conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Package created!");
                }

                //get new pack_id
                int pack_id = -1;
                using (var command = new NpgsqlCommand(new SQL_Statements().getPack_ID(), conn))
                {
                    var res = command.ExecuteReader();
                    if (res.HasRows)
                    {
                        res.Read();
                        pack_id = res.GetInt32(0);
                        Console.WriteLine(pack_id);
                    }
                    Console.Out.WriteLine("fetched package ID for card creation...");
                    res.Close();
                }

                if (pack_id == -1)
                {
                    //return httpresponse with error or exception
                    throw new Exception("pack_id was not edited");
                }

                try
                {   

                    foreach (var card in package)
                    {
                        using (var command = new NpgsqlCommand(new SQL_Statements().addCard(pack_id), conn))
                        {
                            command.Parameters.AddWithValue("@card_id", card.Id);
                            command.Parameters.AddWithValue("@name", card.Name);
                            command.Parameters.AddWithValue("@dmg", card.Damage);
                            command.Parameters.AddWithValue("@pack_id", pack_id);
                            command.ExecuteNonQuery();
                            Console.Out.WriteLine("Card created!");
                        }
                    }
                }
                catch (Npgsql.PostgresException ex)
                {
                    if (ex.ErrorCode == 23505)
                    {
                        Console.WriteLine("Exception: card already exists!\nSending appropriate HTTP Response...");
                        conn.Close();
                        return new Server.HttpResponse().Package409();
                    }
                }
                catch (ArgumentNullException)
                {
                    //give back 
                }
                //create cards one by one
                conn.Close();
                return new Server.HttpResponse().Package201();
            }
        }

        public string buyPackage(string user)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                SQLstatement = new SQL_Statements().searchUser();//get Select Statement which asks for entry by username and password
                SQLstatement = SQLstatement.Substring(0, SQLstatement.Length - 25); //removes password part in select statement
                using (var command = new NpgsqlCommand(SQLstatement, conn))
                {
                    command.Parameters.AddWithValue("@username", user);
                    var reader = command.ExecuteReader();
                    if (reader.HasRows) //hasRows means user from token exists in db
                    {
                        reader.Read();
                        if (reader.GetInt32(5) < 5)
                        {
                            //not enough money
                            return new Server.HttpResponse().BuyPackage403();
                        }
                        Console.WriteLine("get username for package" + reader.GetValue(0));
                    }
                    else
                    {
                        //user not found
                    }
                    reader.Close();
                }

                //get available pack_id
                int pack_id = 0;
                using (var command = new NpgsqlCommand(new SQL_Statements().getAvailablePack(), conn))
                {
                    var reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return new HttpResponse().buyPackage404();
                    }
                    reader.Read();
                    pack_id = reader.GetInt32(0);
                    
                    reader.Close();
                }

                //update bought package to available = false
                using (var command = new NpgsqlCommand(new SQL_Statements().setPackUnavailable(), conn))
                {
                    command.Parameters.AddWithValue("@packID", pack_id);
                    command.ExecuteNonQuery();
                }

                //update cards with username
                using (var command = new NpgsqlCommand(new SQL_Statements().buyPackage(), conn))
                {
                    command.Parameters.AddWithValue("@username", user);
                    command.Parameters.AddWithValue("@packID", pack_id);
                    command.ExecuteNonQuery();
                }

                //subtract users coins by 5
                using (var command = new NpgsqlCommand(new SQL_Statements().userPayCoins(), conn))
                {
                    command.Parameters.AddWithValue("@username", user);
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }

            return new HttpResponse().buyPackage200();
        }

        public string showCards(string user)
        {
            string jsonreply = null;
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var command = new NpgsqlCommand(new SQL_Statements().getCardsFromUser(), conn))
                {
                    command.Parameters.AddWithValue("@username", user);
                    var reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return new HttpResponse().showCards203();
                    }
                    var cards = getCard(reader);
                    jsonreply = JsonConvert.SerializeObject(cards);
                    reader.Close();
                }
                conn.Close();
            }
            return new HttpResponse().showCards200(jsonreply);
        }

        public string showDeck(string user)
        {
            string jsonreply = null;
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var command = new NpgsqlCommand(new SQL_Statements().getDeck(), conn))
                {
                    command.Parameters.AddWithValue("@username", user);
                    var reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return new HttpResponse().emptyDeck203();
                    }
                    var cards = getCard(reader);
                    jsonreply = JsonConvert.SerializeObject(cards);
                    reader.Close();
                }

            }

            return new HttpResponse().showDeck200(jsonreply);
        }

        public string setDeck(string user, DeckJson deck)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                //check if given deck is valid
                using (var command = new NpgsqlCommand(new SQL_Statements().checkGivenDeck(), conn))
                {
                    command.Parameters.AddWithValue("@id1", deck.CardList.ElementAt(0));
                    command.Parameters.AddWithValue("@id2", deck.CardList.ElementAt(1));
                    command.Parameters.AddWithValue("@id3", deck.CardList.ElementAt(2));
                    command.Parameters.AddWithValue("@id4", deck.CardList.ElementAt(3));
                    command.Parameters.AddWithValue("@username", user);
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    { //given deck is not valid
                        reader.Read();
                        if (reader.GetInt32(0) != 4)
                        {
                            conn.Close();
                            return new HttpResponse().setDeck403();
                        }
                    }
                    reader.Close();
                }

                //if deck valid, reset current Deck and update with new one
                using (var command = new NpgsqlCommand(new SQL_Statements().resetDeck(), conn))
                {
                    command.Parameters.AddWithValue("@username", user);
                    command.ExecuteNonQuery();
                }

                using (var command = new NpgsqlCommand(new SQL_Statements().setDeck(), conn))
                {
                    command.Parameters.AddWithValue("@id1", deck.CardList.ElementAt(0));
                    command.Parameters.AddWithValue("@id2", deck.CardList.ElementAt(1));
                    command.Parameters.AddWithValue("@id3", deck.CardList.ElementAt(2));
                    command.Parameters.AddWithValue("@id4", deck.CardList.ElementAt(3));
                    command.Parameters.AddWithValue("@username", user);
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }

            return new HttpResponse().setDeck200();
        }

        public string showUserData(string user)
        {
            string jsonreply = null;
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(new SQL_Statements().getUser(), conn))
                {
                    command.Parameters.AddWithValue("@username", user);
                    var reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return new HttpResponse().getUser404();
                    }
                    reader.Read();
                    UsersJson userjson = new UsersJson();
                    userjson.Name = reader.GetValue(0).ToString();
                    userjson.Bio = reader.GetValue(1).ToString();
                    userjson.Image = reader.GetValue(2).ToString();
                    jsonreply = JsonConvert.SerializeObject(userjson);
                }
            }

            return new HttpResponse().getUser200(jsonreply);
        }

        public string editUserData(string user, UsersJson userdata)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(new SQL_Statements().editUser(), conn))
                {
                    command.Parameters.AddWithValue("@newName", userdata.Name);
                    command.Parameters.AddWithValue("@newBio", userdata.Bio);
                    command.Parameters.AddWithValue("@newImage", userdata.Image);
                    command.Parameters.AddWithValue("@username", user);
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }

            return new HttpResponse().putUser200();
        }

        public string showStats(string user)
        {
            string jsonreply = null;
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(new SQL_Statements().getStats(), conn))
                {
                    command.Parameters.AddWithValue("@username", user);
                    var reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        //maybe?
                    }
                    reader.Read();
                    StatsJson userstats = new StatsJson();
                    userstats.Name = reader.GetString(0);
                    userstats.Elo = reader.GetInt32(1);
                    userstats.Wins = reader.GetInt32(2);
                    userstats.Losses = reader.GetInt32(3);
                    jsonreply = JsonConvert.SerializeObject(userstats);
                }
                conn.Close();
            }

            return new HttpResponse().getStats200(jsonreply);
        }

        private List<CardsJson> getCard(NpgsqlDataReader reader)
        {
            List<CardsJson> reply = new List<CardsJson>();
            while (reader.Read())
            {
                CardsJson card = new CardsJson();
                card.Id = reader.GetValue(0).ToString();
                card.Name = reader.GetValue(1).ToString();
                card.Damage = reader.GetFloat(2);
                reply.Add(card);
            }
            return reply;
        }
    }
}