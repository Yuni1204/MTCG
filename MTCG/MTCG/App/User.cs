using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.App
{
    internal class User
    {
        public string Username { get; set; }
        public int Coins = 0;
        public int Elo;

        public List<Card> Stack;
        public List<Card> Deck;

        public User(string uname)
        {
            this.Username = uname;
            this.Coins = 5;
            this.Elo = 100;
            
        }
    }
}
