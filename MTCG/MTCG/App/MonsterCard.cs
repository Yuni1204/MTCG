using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
    public class MonsterCard : Card
    {
        public MonsterCard(string cardId, string cardname, float carddmg)
        {
            Id = cardId;
            Name = cardname;
            Type = "MonsterCard";
            Element = setElement();
            Dmg = carddmg;
        }

        
        //public override void Interact(string targetname)
        //{
        //    Console.WriteLine($"{targetname} du hurensohn!");
        //}
    }
}
