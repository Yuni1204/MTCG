using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
   public abstract class Card
   {
       private string _id;
       private string _name;
       private string _type;
       private string _element;
       private float _dmg;

       public string Id
       {
           get => _id;
           set => _id = value;
        }

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

       public string Element
       {
           get => _element;
           set => _element = value;
       }

       public float Dmg
       {
           get => _dmg;
           set => _dmg = value;
       }

       protected string setElement()
       {
           if (Name.Contains("Fire"))
           {
               return "Fire";
           } else if (Name.Contains("Water"))
           {
               return "Water";
           } else
           {
               return "Normal";
           }
       }

   }
}
