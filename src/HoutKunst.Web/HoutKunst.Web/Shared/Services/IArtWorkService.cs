using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoutKunst.Web.Shared.Services
{
    public interface IArtWorkService
    {
        Task<IEnumerable<ArtWorkDto>> GetIndexAsync();
        Task<ArtWorkDto> GetDetailAsync(int artId);
        Task DeleteAsync(int artid);
    }
}
