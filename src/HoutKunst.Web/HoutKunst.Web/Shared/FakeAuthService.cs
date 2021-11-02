using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HoutKunst.Web.Shared
{
    public class FakeAuthService : AuthenticationStateProvider
    {
        public static ClaimsPrincipal Anonymous => new(new ClaimsIdentity());

        public static ClaimsPrincipal Administrator =>
     new(new ClaimsIdentity(new[]
     {
     new Claim(ClaimTypes.Name, "Administrator"),
     new Claim(ClaimTypes.Email, "fake-administrator@gmail.com"),
     new Claim(ClaimTypes.Role, "Administrator"),
     }, "Fake Authentication"));
        public static ClaimsPrincipal Customer =>
             new(new ClaimsIdentity(new[]
             {
         new Claim(ClaimTypes.Name, " Customer"),
         new Claim(ClaimTypes.Email, "fake-customer@gmail.com"),
         new Claim(ClaimTypes.Role, "Customer"),
             }, "Fake Authentication"));
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(Administrator));
        }
    }
}
