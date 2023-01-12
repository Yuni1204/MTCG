using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
    internal class Battle
    {
        public Card Card1;
        public Card Card2;
        public Battle()
        {
            Random rng = new Random();
            //var Card1 = new MonsterCard("Name", "Type");
            //var Card2 = new MonsterCard("Name2", "Type2");
            switch (rng.Next(2))
            {
                case 0:
                    Card1 = new MonsterCard("Monster1");
                    break;
                case 1:
                    Card1 = new SpellCard("Spell1");
                    break;
            }
            switch (rng.Next(2))
            {
                case 0:
                    Card2 = new MonsterCard("Monster1");
                    break;
                case 1:
                    Card2 = new SpellCard("Spell1");
                    break;
            }

        }
        public int integer = (int)cardName.Goblin;
        public void Beschimpfen()
        {
            Card1.Interact(Card2.Name);
            //Console.WriteLine($"{Card1.Name} beschimpft {Card2.Name}: {Card1.Interact(Card2.Name)}");
            Card2.Interact(Card1.Name);
        }

        public void Fight(Card c1, Card c2)
        {

        }

        public void CardEffect(Card c1, Card c2)
        {

        }
    }
}
