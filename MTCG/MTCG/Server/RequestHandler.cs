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
            string user = null;
            var request = data.Split('\n');
            int rlen = request.Length;
            var httpinfo = request[0].Split(' ');
            string Authmsg = null;
            string subdir = httpinfo[1];
            Console.WriteLine(subdir);
            if (subdir.Length > 6)
            {
                if (subdir.Remove(6) == "/users")
                {
                    Console.WriteLine(subdir);
                    httpinfo[1] = "/users";
                }
            }
            switch (httpinfo[1])
            {
                case "/users": //get userinfo, check if already exists, if not add to db
                    if (subdir != "/users")
                    {
                        return AuthHandler(request, subdir.Substring(7), request.Length, httpinfo[1], httpinfo[0]);
                    }
                    var userdata = JsonConvert.DeserializeObject<UsersTable>(request[rlen - 1]);
                    return db.addUser(userdata);
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
                    return AuthHandler(request, "admin", request.Length, httpinfo[1]/*"/packages"*/, httpinfo[0]);
                    break;
                case "/transactions/packages":
                    return AuthHandler(request, null, request.Length, httpinfo[1]/*"/transactions/packages"*/, httpinfo[0]);
                    break;
                case "/cards":
                    return AuthHandler(request, null, request.Length, httpinfo[1]/*"/cards"*/, httpinfo[0]);
                    break;
                case "/deck":
                    return AuthHandler(request, null, request.Length, httpinfo[1] /*"/deck"*/, httpinfo[0]);
                        break;
                case "/stats":
                    return AuthHandler(request, null, request.Length, httpinfo[1] /*/stats*/, httpinfo[0]);
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
            if ((tokenuser == authorizedUser) || (tokenuser) == "admin")
            { //check if user is logged in
                if(tokenuser == "admin") { return true; }
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

        public string AuthHandler(string[] request, string Authuser, int rlen, string subdir, string method)
        {
            string user = null;
            string Authmsg = null;
            (Authmsg, user) = checkAuth(getAuthToken(request), Authuser);
            if (Authmsg == "noAuthToken")
            {
                return new HttpResponse().Unauthorized401();
            }
            if (Authmsg == "valid")
            { //depending on subdir, return the right responses
                switch (subdir)
                {
                    case "/packages":
                        //then create packages
                        var package = JsonConvert.DeserializeObject<List<CardsJson>>(request[rlen - 1]);
                        return db.addPackage(package);
                        break;
                    case "/transactions/packages":
                        return db.buyPackage(user);
                        break;
                    case "/cards":
                        return db.showCards(user);
                        break;
                    case "/deck":
                        if (method == "GET")
                        {
                            return db.showDeck(user);
                        }
                        if (method == "PUT")
                        {
                            var deck = parseDeck((request[rlen - 1]));
                            //Console.WriteLine(deck.CardList.Count);
                            if (!(deck.CardList.Count == 4))
                            {
                                return new HttpResponse().notEnoughCardsForDeck400();
                            }
                            return db.setDeck(user, deck);
                        }
                        break;
                    case "/users": //this is /users/{username}
                        if (method == "GET")
                        {
                            return db.showUserData(user);
                        }
                        if (method == "PUT")
                        {
                            var userdata = JsonConvert.DeserializeObject<UsersJson>(request[rlen - 1]);
                            return db.editUserData(user, userdata);
                        }
                        break;
                    case "/stats":
                        return db.showStats(user);
                        break;
                }
                
            }
            else if (Authmsg == "invalid")
            {
                return new HttpResponse().notAuthUser403(Authuser);
            }
            else
            {
                Console.WriteLine("case /packages error");
            }

            return "hello world";
        }

        private DeckJson parseDeck(string json)
        {
            DeckJson result = new DeckJson();

            var ids = json.Split(',');
            foreach (var id in ids)
            {
                result.CardList.Add(id.Trim().TrimStart('[', '"').TrimEnd(']', '"'));
                Console.WriteLine(result.CardList.Last());
            }

            return result;
        }
    }

}
