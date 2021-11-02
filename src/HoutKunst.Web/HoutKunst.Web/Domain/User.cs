using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public enum Rol { 
    Admin,
    Klant,
    Kunstenaar
    }
    public class User 
    {
        public string Name { get; set; }
        public Rol  Rol { get; set; }
        public int Id { get; set; }

        public User()
        {
            Name = "het werkt";
        }
    }
}
