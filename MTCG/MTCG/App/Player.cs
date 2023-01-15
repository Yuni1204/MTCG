using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.App
{
    public class Player
    {
        public string Username;
        public string Name;
        public int Elo;
        public int Wins;
        public int Losses;
        public List<Card> Deck = new List<Card>();

    }
}
