using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MTCG.DB;
using Newtonsoft.Json.Linq;
using MTCG.json;
using Newtonsoft.Json;

namespace MTCG.Server
{
    internal class RequestHandler
    {
        public List<string> LoggedInList = new List<string>();
        private DB.DataBase db = new DataBase();
        public string ParseHttpRequest(string data)
        {
            var request = data.Split('\n');
            int rlen = request.Length;
            var httpinfo = request[0].Split(' ');
            switch (httpinfo[1])
            {
                case "/users": //get userinfo, check if already exists, if not add to db
                    var userdata = JsonConvert.DeserializeObject<Users>(request[rlen - 1]);
                    if (!db.alreadyExists("users", (string)userdata.Username))
                    {
                        return db.addUser(userdata);
                    }
                    break;
                case "/sessions":
                    userdata = JsonConvert.DeserializeObject<Users>(request[rlen - 1]);
                    var result = db.searchUser(userdata);
                    if (getResponseCode(result) == "200")
                    {
                        LoggedInList.Add(userdata.Username);
                    }
                    return result;
                default:
                    //Console.WriteLine("http request handler switch default");
                    break;
            }
            //Console.WriteLine(request[rlen-1]);
            //foreach (var line in request)
            //{

            //    Console.WriteLine(line);
            //}
            return "400;";
        }

        public string getResponseCode(string response)
        {
            var rescode = response.Split(' ');
            return rescode[1];
        }

        //public string jsonToDynamic(string json)
        //{
        //    dynamic userdata = JsonObject.Parse(json);
        //    return
        //}
    }

}
