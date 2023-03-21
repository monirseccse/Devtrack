using System.Security.Claims;

namespace DevTrack.Infrastructure.Services
{
    public interface ITokenService
    {
        string GetToken(List<Claim> claims);
    }
}
