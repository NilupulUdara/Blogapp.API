using Microsoft.AspNetCore.Identity;

namespace Blogapp.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTTtoken(IdentityUser user, List<string> roles);
    }
}