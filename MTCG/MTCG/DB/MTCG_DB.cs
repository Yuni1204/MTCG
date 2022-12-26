using Npgsql;
using System;
using System.Text.Json.Nodes;
using MTCG.json;
using static System.Net.WebRequestMethods;

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

        public bool alreadyExists(string table, string value)
        {
            return false;
        }

        public string addUser(JsonObject userdata)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                SQLstatement = "CREATE TABLE IF NOT EXISTS users" +
                               "(username VARCHAR(50) PRIMARY KEY, password VARCHAR(50))";
                using (var command = new NpgsqlCommand(SQLstatement, conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Finished creating table 'User' if not exists");
                }

                SQLstatement = new SQL_Statements().insertInto("users", new string[] {"username", "password"}, 
                    new string[] { (string)userdata["Username"], (string)userdata["Password"] } );

                //"INSERT INTO users (username, password) " +
                //           "VALUES('" + userdata["Username"] + "', '" + userdata["Password"] + "')"; 
                try
                {
                    using (var command = new NpgsqlCommand(SQLstatement, conn))
                    {
                        command.ExecuteNonQuery();
                        Console.Out.WriteLine("Added "+userdata["Username"]+" into DB!");
                    }
                }
                catch (Npgsql.PostgresException ex)
                {
                    if (ex.Code == "23505")
                    {
                        return new Server.HttpResponse().HttpResp409();
                    }
                    Console.WriteLine("some other Exception in PostgresExc");
                }

                conn.Close();
            }

            return new Server.HttpResponse().HttpResp201();
            //return new Server.HttpResponse().testmethod();
            //"HTTP / 1.1 200 OK \r"
        }
        /*
        public void DBConnect()
        {
            // Build connection string using parameters from portal
            //
            string connString =
                String.Format(
                    "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer",
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);


            using (var conn = new NpgsqlConnection(connString))

            {
                Console.Out.WriteLine("Opening connection");
                conn.Open();

                using (var command = new NpgsqlCommand("DROP TABLE IF EXISTS inventory", conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Finished dropping table (if existed)");

                }

                using (var command = new NpgsqlCommand("CREATE TABLE inventory(id serial PRIMARY KEY, name VARCHAR(50), quantity INTEGER)", conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Finished creating table");
                }

                using (var command = new NpgsqlCommand("INSERT INTO inventory (name, quantity) VALUES (@n1, @q1), (@n2, @q2), (@n3, @q3)", conn))
                {
                    command.Parameters.AddWithValue("n1", "banana");
                    command.Parameters.AddWithValue("q1", 150);
                    command.Parameters.AddWithValue("n2", "orange");
                    command.Parameters.AddWithValue("q2", 154);
                    command.Parameters.AddWithValue("n3", "apple");
                    command.Parameters.AddWithValue("q3", 100);

                    int nRows = command.ExecuteNonQuery();
                    Console.Out.WriteLine(String.Format("Number of rows inserted={0}", nRows));
                }
            }

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();
        }
        */
    }
}