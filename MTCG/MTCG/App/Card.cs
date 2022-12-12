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

        //Card(string newname, string newtype)
        //{
        //    Name = newname;
        //    Type = newtype;
        //}

        public abstract void Interact(string targetname);


   }
}
