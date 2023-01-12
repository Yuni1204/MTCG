using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MTCG.App;
using MTCG.DB;
using Newtonsoft.Json.Linq;
using MTCG.json;
using Newtonsoft.Json;

namespace MTCG.Server
{
    internal class RequestHandler
    {
        //UPDATE LoggedInUser to LIST<User>
        public List<string> LoggedInUsers = new List<string>();
        private DB.DataBase db = new DataBase();
        public string ParseHttpRequest(string data)
        {
            var request = data.Split('\n');
            int rlen = request.Length;
            var httpinfo = request[0].Split(' ');
            string Authmsg = null;
            switch (httpinfo[1])
            {
                case "/users": //get userinfo, check if already exists, if not add to db
                    var userdata = JsonConvert.DeserializeObject<UsersTable>(request[rlen - 1]);
                    if (!db.alreadyExists("users", (string)userdata.Username))
                    {
                        return db.addUser(userdata);
                    }
                    break;
                case "/sessions":
                    userdata = JsonConvert.DeserializeObject<UsersTable>(request[rlen - 1]);
                    var result = db.searchUser(userdata);
                    var testest = getResponseCode(result);
                    if (getResponseCode(result) == "200")
                    {
                        LoggedInUsers.Add(userdata.Username);
                    }
                    return result;
                case "/packages":
                    //first, check if admin
                    (Authmsg, string str) = checkAuth(getAuthToken(request), "admin");
                    if (Authmsg == "noAuthToken")
                    {
                        return new HttpResponse().Unauthorized401();
                    }
                    if (Authmsg == "valid")
                    { //then create packages
                        var package = JsonConvert.DeserializeObject<List<CardsJson>>(request[rlen - 1]);
                        return db.addPackage(package);
                    }
                    else if(Authmsg == "invalid")
                    {
                        return new HttpResponse().Package403();
                    }
                    else
                    {
                        Console.WriteLine("case /packages error");
                    }
                    break;
                case "/transactions/packages":
                    (Authmsg, string user) = checkAuth(getAuthToken(request), null);
                    if (Authmsg == "noAuthToken")
                    {
                        return new HttpResponse().Unauthorized401();
                    }
                    if (Authmsg == "valid")
                    { //authuser 
                        return db.buyPackage(user);
                    }
                    else if (Authmsg == "invalid")
                    {
                        return new HttpResponse().noLoginPackage403();
                    }
                    break;
                default:
                    //Console.WriteLine("http request handler switch default");
                    break;
            }
            return "400;";
        }

        public string getResponseCode(string response)
        {
            var rescode = response.Split(' ');
            return rescode[1];
        }

        public string getAuthToken(string[] request)
        {
            foreach (var line in request)
            {
                var words = line.Split(' ');
                foreach (var word in words)
                {
                    if (word == "Authorization:")
                    { //remove "\r" from end of string, then return token e.g. admin-mtcgToken\r -> admin-mtcgToken
                        return words[(words.Length) - 1].Substring(0, words[(words.Length) - 1].Length-1);
                    }
                }
            }
            return null;
        }

        public bool validToken(string token, string authorizedUser)
        {
            string tokenuser = getUserFromToken(token);
            if (authorizedUser == null)
            { //
                authorizedUser = tokenuser;
            }
            if (tokenuser == authorizedUser)
            { //check if user is logged in
                foreach (var username in LoggedInUsers.ToList()) //ToList könnte schon das problem lösen, ansonsten zb lokal liste in einer variable speichern
                {
                    if (username == tokenuser)
                    {//check if Token has valid format
                        if (token == (tokenuser + "-mtcgToken"))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public string getUserFromToken(string token)
        {
            var res = token.Split('-');
            return res[0];
        }

        public (string, string) checkAuth(string AuthToken, string AuthorizedUser)
        {
            if (AuthToken == null)
            { //http response with unauthorized error
                return ("noAuthToken", null);
            }
            if (validToken(AuthToken, AuthorizedUser))
            {
                return ("valid", getUserFromToken(AuthToken));
            }
            else
            {
                return ("invalid", null);
            }
        }
        
    }

}
