using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IUserRepo
    {
        List<User> GetAll();
        User GetById(int id);

        List<User> GetByRole(Rol rol);
        void Add(User user);
        void Delete(User user);
        void Update(User userk);
        void SaveChanges();
    }
}
