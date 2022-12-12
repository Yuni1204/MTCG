using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
    class MonsterCard : Card
    {
        public MonsterCard(string newname)
        {
            Name = newname;
            Type = "MonsterCard";
        }
        public override void Interact(string targetname)
        {
            Console.WriteLine($"{targetname} du hurensohn!");
        }
    }
}
