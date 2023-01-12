using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
   abstract class Card
   {
       private string _name;
       private string _type;
       private int _dmg;

       public string Name
       {
           get => _name; 
           set => _name = value;
       }

       public string Type
       {
           get => _type;
           set => _type = value;
       }

       public int Dmg
       {
           get => _dmg;
           set => _dmg = value;
       }

        //Card(string newname, string newtype)
        //{
        //    Name = newname;
        //    Type = newtype;
        //}

        public abstract void Interact(string targetname);


   }
}
