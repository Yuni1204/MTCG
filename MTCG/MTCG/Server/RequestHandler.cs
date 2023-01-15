using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
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
        private readonly object _lock = new object();
        public List<string> LoggedInUsers = new List<string>();
        private DB.DataBase db = new DataBase();
        private Lobby GameLobby = new Lobby();
        private bool only1PlayerDone = false;
        private bool gameRunning = false;
        public int eloP1Before;
        public int eloP2Before;

        public void LoginHighEloGamer()
        {
            LoggedInUsers.Add("HighEloGamer");
        }

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
                case "/score":
                    return AuthHandler(request, null, request.Length, httpinfo[1] /*/score*/, httpinfo[0]);
                    break;
                case "/battles":
                    return AuthHandler(request, getUserFromToken(getAuthToken(request)), request.Length, httpinfo[1] /*/score*/, httpinfo[0]);
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
                            //var userdata = JsonConvert.DeserializeObject<UsersJson>(request[rlen - 1]);
                            return db.editUserData(user, JsonConvert.DeserializeObject<UsersJson>(request[rlen - 1]));
                        }
                        break;
                    case "/stats":
                        return db.showStats(user);
                        break;
                    case "/score":
                        return db.showScoreboard();
                        break;
                    case "/battles":
                        
                        if (playerAlreadyInQ(user))
                        {
                            return new HttpResponse().tooManyRequests429();
                        }
                        lock (_lock)
                        {
                            GameLobby.PlayerQ.Add(db.getPlayerForBattle(user));
                        }
                        while (inQueue(user)) //no opponent
                        {
                            Thread.Sleep(1000);
                        }
                        Player p1 = new Player();
                        Player p2 = new Player();
                        List<string> battleLog = new List<string>();
                        lock (_lock)
                        {
                            p1 = GameLobby.PlayerQ.ElementAt(0);
                            p2 = GameLobby.PlayerQ.ElementAt(1);
                            eloP1Before = p1.Elo;
                            eloP2Before = p2.Elo;
                        }
                        gameRunning = true;
                        (p1, p2, battleLog)  = GameLobby.startBattle(p1, p2);
                        gameRunning = false;
                        lock (_lock)
                        {
                            if (only1PlayerDone)
                            {
                                only1PlayerDone = false;
                            }
                            else if (!only1PlayerDone)
                            {
                                only1PlayerDone = true;
                            }
                        }
                        if (p1.Username == user)
                        {
                            while (only1PlayerDone)
                            {
                                Thread.Sleep(1000);
                            }
                            lock (_lock)
                            {
                                GameLobby.PlayerQ.Remove(p1);
                            }
                            //update stats
                            //Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n" + GameLobby.PlayerQ.Count);
                            if (eloDiff(eloP1Before, eloP2Before) > 25)
                            {
                                if (getLowerEloPlayer(eloP1Before, eloP2Before)) //true means elo2 is lowerEloPlayer, when false -> elo1
                                {
                                    db.rewardLowerEloPlayer(p2.Username);
                                }
                                else
                                {
                                    db.rewardLowerEloPlayer(p1.Username);
                                }
                            }
                            return db.updatePlayerData(p1, battleLog);
                        }
                        else if (p2.Username == user)
                        {
                            while (only1PlayerDone)
                            {
                                Thread.Sleep(1000);
                            }
                            lock (_lock)
                            {
                                GameLobby.PlayerQ.Remove(p2);
                            }
                            //update stats

                            //Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n" + GameLobby.PlayerQ.Count);
                            return db.updatePlayerData(p2, battleLog);
                        }
                        //if (GameLobby.matchupAvailable() && playerForNextMatch(user))
                        //{
                        //    GameLobby.startBattle(GameLobby.PlayerQ.ElementAt(0), GameLobby.PlayerQ.ElementAt(1));
                        //}
                        //else
                        //{
                        //    //wait
                        //}
                        //return 
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

        public int eloDiff(int elo1, int elo2)
        {
            if (elo1 > elo2)
            {
                return elo1 - elo2;
            }
            else
            {
                return elo2 - elo1;
            }
        }

        private bool playerAlreadyInQ(string user)
        {
            if (GameLobby.PlayerQ.Count > 0)
            {
                foreach (var player in GameLobby.PlayerQ)
                {
                    if (player.Username == user)
                    {
                        // player is already waiting in lobby
                        return true;
                    }
                }
            }
            return false;
        }

        private bool playerForNextMatch(string user)
        {
            if ((GameLobby.PlayerQ.ElementAt(0).Username == user) ||
                (GameLobby.PlayerQ.ElementAt(1).Username == user))
            {
                return true;
            }
            return false;
        }

        private bool inQueue(string user)
        {
            if (!gameRunning && playerForNextMatch(user) && (GameLobby.PlayerQ.Count >= 2))
            {
                return false;
            }
            return true;
        }

        public bool getLowerEloPlayer(int elo1, int elo2)
        {
            return elo1 > elo2; //true means elo2 is lowerEloPlayer, when false -> elo1
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
