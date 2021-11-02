using Domain;
using HoutKunst.Web.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;

namespace HoutKunst.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _users;


        public UserController(IUserRepo users)
        {
            this._users = users;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> Get() {

            List<UserDto> usersdt = _users.GetAll().Select(u => new UserDto(u)).ToList();

            return usersdt;
            

        
        

        }


    }
}
