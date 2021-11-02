using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoutKunst.Web.Shared
{
    public class FakeUserService : IUserService
    {
        private static readonly List<UserDto> users = new();

        static FakeUserService() {

            var userIDs = 0;
            var UserFaker = new Faker<UserDto>("nl")
                .UseSeed(1337)
                .RuleFor(u => u.Name, f => f.Name.FirstName())
                .RuleFor(u => u.Image, f => f.Internet.Avatar());

            users = UserFaker.Generate(10);

                }

       
        public Task<IEnumerable<UserDto>> GetIndexAsync(string name = "")
        {
            return Task.FromResult(users.Where(u=> u.Name.Contains(name,StringComparison.OrdinalIgnoreCase)).AsEnumerable());
        }
    }
}
