using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace HoutKunst.Web.Server.Data.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext context;
        private DbSet<User> users;
        public UserRepo(ApplicationDbContext contex)
        {
            this.context = contex;
            users = context.users;

        }
        public void Add(User user)
        {
            users.Add(user);
        }

        public void Delete(User user)
        {
            users.Remove(user);
        }

        public List<User> GetAll()
        {
            return users.ToList();
        }

        public User GetById(int id)
        {
            return users.Where(u => u.Id == id).FirstOrDefault();
        }

        public List<User> GetByRole(Rol rol)
        {
            return users.Where(u => u.Rol == rol).ToList();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void Update(User user)
        {
            users.Update(user);
        }
    }
}
