using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.json;

namespace MTCG.DB
{
    internal class SQL_Statements
    {
        public string connectValuesToOneString(string[] values)
        {
            string valuesreply = null;
            for (int i = 0; i < values.Length; i++)
            {
                if (i == values.Length - 1)
                {//last value does not need ","
                    valuesreply += "'" + values[i] + "'";
                    break;
                }
                valuesreply += "'" + values[i] + "',";
            }
            return valuesreply;
        }

        public string createUser()
        {
            return "INSERT INTO users (username,password,name) VALUES (@username,@password,@username)";
        }

        public string searchUser()
        {
            return "SELECT * from users WHERE username = @username AND password = @password";
        }

        public string allTables()
        {
            return "SELECT table_name FROM information_schema.tables WHERE table_schema='public' AND table_type='BASE TABLE';";
        }

        public string truncateTable(string table)
        {
            return $"TRUNCATE TABLE {table} CASCADE";
        }

        public string createTablePackages()
        {
            return "CREATE TABLE IF NOT EXISTS packages (pack_id serial PRIMARY KEY, available boolean DEFAULT true)";
        }

        public string createTableCards()
        {
            return "CREATE TABLE IF NOT EXISTS cards (card_id character(36) NOT NULL, card_name character varying(16) NOT NULL, " +
                   "card_dmg real NOT NULL, pack_id integer, username character varying(50), deck boolean DEFAULT false, PRIMARY KEY(card_id), CONSTRAINT pack_id FOREIGN KEY(pack_id) " +
                   "REFERENCES public.packages(pack_id), CONSTRAINT username FOREIGN KEY(username) REFERENCES public.users(username))";
        }

        public string addPackage()
        {
            return "INSERT INTO packages DEFAULT VALUES";
        }

        public string getPack_ID()
        {
            return "SELECT lastval()";
        }

        public string checkForDuplicateCards()
        {
            return
                "SELECT * FROM cards WHERE card_id = @id1 OR card_id = @id2 OR card_id = @id3 OR card_id = @id4 OR card_id = @id5";
        }

        public string addCard(int pack_id)
        {
            return "INSERT INTO cards (card_id, card_name, card_dmg, pack_id) VALUES (@card_id, @name, @dmg, @pack_id)";
        }

        public string getAvailablePack()
        {
            return "SELECT pack_id FROM packages WHERE available = true";
        }

        public string setPackUnavailable()
        {
            return "UPDATE packages SET available = false WHERE pack_id = @packID";
        }

        public string buyPackage()
        {
            return "UPDATE cards SET username = @username WHERE pack_id = @packID";
        }

        public string userPayCoins()
        {
            return "UPDATE users SET coins = coins - 5 WHERE username = @username";
        }

        public string getCardsFromUser()
        {
            return "SELECT card_id, card_name, card_dmg FROM cards WHERE username = @username";
        }

        public string getDeck()
        {
            return "SELECT card_id, card_name, card_dmg FROM cards WHERE username = @username AND deck = true";
        }

        public string checkGivenDeck()
        {
            return "SELECT COUNT(card_id) FROM cards WHERE username = @username AND (card_id = @id1 OR card_id = @id2 OR card_id = @id3 OR card_id = @id4)";
        }

        public string setDeck()
        {
            return "UPDATE cards SET deck = true WHERE username = @username AND (card_id = @id1 OR card_id = @id2 OR card_id = @id3 OR card_id = @id4)";
        }

        public string resetDeck()
        {
            return "UPDATE cards SET deck = false WHERE username = @username";
        }

        public string getUser()
        {
            return "SELECT username, bio, image FROM users WHERE username = @username";
        }

        public string editUser()
        {
            return "UPDATE users SET name = @newName, bio = @newBio, image = newImage WHERE username = @username";
        }
    }
}
