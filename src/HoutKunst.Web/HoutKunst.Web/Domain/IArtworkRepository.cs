using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IArtworkRepository
    {
        List<Artwork> GetAll();
        Artwork GetById(int id);
        void Add(Artwork artwork);
        void Delete(Artwork artwork);
        void Update(Artwork artwork);
        void SaveChanges();
    }
}
