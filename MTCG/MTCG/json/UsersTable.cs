using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.json
{
    public class UsersTable
    {
        public string Username;
        public string Password;
        public string Bio;
        public string Image;
        public int Coins;
        public int Elo;
        public int GamesPlayed;
        public int Wins;
        public int Losses;
        public int Draws;


        //public string username
        //{
        //    get { return username; }
        //    set
        //    {

        //    }
        //}

        public UsersTable(/*List<string> data*/)
        {
                Username = null;
                Password = null;
                Bio = null;
                Image = null;
                Coins = 20;
                Elo = 100;
                GamesPlayed = 0;
                Wins = 0;
                Losses = 0;
                Draws = 0;
            //if (data == null)
            //{
            //}
            //else if(data.Count == 10)
            //{
            //    Username = data[0];
            //    Password = data[1];
            //    Bio = data[2];
            //    Image = data[3];
            //    Coins = int.Parse(data[4]);
            //    Elo = int.Parse(data[5]);
            //    GamesPlayed = int.Parse(data[6]);
            //    Wins = int.Parse(data[7]);
            //    Losses = int.Parse(data[8]);
            //    Draws = int.Parse(data[9]);
                    
            //}

        }

        public void updateUserData(/*dictionary*/)
        {

        }
    }
}
