using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoutKunst.Web.Shared
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Image { get; set; }


        public UserDto()
        {

        }
        public UserDto(User u)
        {
            this.Name = u.Name;
           

        }
    }
}
