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
            return "INSERT INTO users (username,password) VALUES (@username,@password)";
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
            return "CREATE TABLE IF NOT EXISTS packages (pack_id serial PRIMARY KEY)";
        }

        public string createTableCards()
        {
            return "CREATE TABLE IF NOT EXISTS cards (card_id character(36) NOT NULL, card_name character varying(16) NOT NULL, " +
                   "card_dmg real NOT NULL, pack_id integer, username character varying(50), PRIMARY KEY(card_id), CONSTRAINT pack_id FOREIGN KEY(pack_id) " +
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
    }
}
