using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DB
{
    internal class SQL_Statements
    {
        public string insertInto(string table, string[] columns, string[] values)
        {
            string columnsreply = null;
            string valuesreply = null;
            for(int i = 0; i < columns.Length; i++)
            {
                if (i == columns.Length - 1)
                {
                    columnsreply += columns[i];
                    valuesreply += "'" + values[i];
                    break;
                }
                if(i == 0) { valuesreply += values[i] + "', "; }
                else { valuesreply += "'" + values[i] + "', "; }
                columnsreply += columns[i] + ",";
            }
            
            return $"INSERT INTO {table} ({columnsreply}) VALUES('{valuesreply}')";
        }
    }
}
