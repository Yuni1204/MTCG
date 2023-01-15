using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MTCG.Server;

namespace MTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //List<Card> Player1 = new List<Card>();
            //Player1.Add(new MonsterCard("1", "FireElve", 1));
            //Player1.Add(new MonsterCard("2", "FireElve", 2));
            //Player1.Add(new MonsterCard("3", "FireElve", 3));
            //Player1.Add(new MonsterCard("4", "FireElve", 4));
            //List<Card> Player2 = new List<Card>();
            //Player2.Add(new MonsterCard("5", "Dragon", 5));
            //Player2.Add(new MonsterCard("6", "Dragon", 6));
            //Player2.Add(new MonsterCard("7", "Dragon", 7));
            //Player2.Add(new MonsterCard("8", "Dragon", 8));
            //var testbattle = new Battle();
            //testbattle.cardBattle(Player1, Player2);
            //foreach (var round in testbattle.log)
            //{
            //    Console.WriteLine(round);
            //}

            MyTcpListener server = new MyTcpListener();
            server.StartServer();

        }
    }
}
