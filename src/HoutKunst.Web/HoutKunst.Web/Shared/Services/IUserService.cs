using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoutKunst.Web.Shared
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetIndexAsync(string name ="");
    }
}
