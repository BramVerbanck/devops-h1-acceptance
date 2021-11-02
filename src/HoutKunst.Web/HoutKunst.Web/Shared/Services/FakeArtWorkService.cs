using Bogus;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoutKunst.Web.Shared.Services
{
    public class FakeArtWorkService : IArtWorkService
    {
        private static readonly List<ArtWorkDto> artworks = new();

        static FakeArtWorkService() {

            var artid = 0;

            var artFaker = new Faker<ArtWorkDto>("nl")
                .UseSeed(1337)
                .RuleFor(a => a.Id, _ => ++artid)
                .RuleFor(a => a.info, f => f.Commerce.ProductName())
                .RuleFor(a => a.Image, f => f.Image.Image())
                .RuleFor(a => a.Maker, f => f.Name.FirstName())
                .RuleFor(a => a.Vraagprijs, f => f.Random.Int(0, 250))
                .RuleFor(a => a.MaakJaar, f => f.Date.Past(2).Year)
                .RuleFor(a => a.IsOutdoors, f => f.Random.Bool())
                .RuleFor(a => a.ArtType, f => f.PickRandom<ArtType>());
                
                

            artworks = artFaker.Generate(10);
        }

        public Task DeleteAsync(int artid)
        {
            var p = artworks.Where(a => a.Id == artid).First();
            artworks.Remove(p);
            return Task.CompletedTask;
        }

        public Task<ArtWorkDto> GetDetailAsync(int artId)
        {
            return Task.FromResult(artworks.Where(a => a.Id == artId).First());
        }

        public Task<IEnumerable<ArtWorkDto>> GetIndexAsync()
        {
            return Task.FromResult(artworks.AsEnumerable());
        }
    }
}
