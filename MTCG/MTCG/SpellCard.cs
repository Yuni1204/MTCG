using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
    class SpellCard : Card
    {
        public SpellCard(string newname)
        {
            Name = newname;
            Type = "SpellCard";
        }
        public override void Interact(string targetname)
        {
            Console.WriteLine($"{targetname} du gaesekki!");
        }
    }
}
