using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTCG.App
{
    internal class Lobby
    {
        public List<Player> PlayerQ = new List<Player>();
        public bool statsUpdated = false;

        public bool matchupAvailable()
        {
            if (PlayerQ.Count > 1)
            {
                if (queueNoDoubleEntries())
                {
                    return true;
                }
            }
            return false;
        }

        public bool queueNoDoubleEntries()
        {
            foreach (var player in PlayerQ)
            {
                foreach (var checkplayer in PlayerQ)
                {

                }
            }
            return true;
        }

        public (Player p1, Player p2, List<string> log) startBattle(Player p1, Player p2)
        {
            Battle Game = new Battle();
            (int winner, List<string> log) = Game.cardBattle(p1.Deck, p2.Deck);
            if (!statsUpdated)
            {
                if (winner == 1)
                {
                    p1.Elo += 3;
                    p1.Wins++;
                    p2.Elo += 5;
                    p2.Losses++;
                }
                else if (winner == 2)
                {
                    p2.Elo += 3;
                    p2.Wins++;
                    p1.Elo += 5;
                    p1.Losses++;
                }
                else
                {
                    //draw
                }
                statsUpdated = true;
            }
            else
            {
                statsUpdated = false;
            }
            return (p1, p2, log);
        }
    }
}
