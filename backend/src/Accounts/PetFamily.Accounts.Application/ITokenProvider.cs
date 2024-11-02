
using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;
using System.Security.Claims;

namespace PetFamily.Accounts.Application
{
    public interface ITokenProvider
    {
        JwtTokenResult GenerateAccessToken(User user, CancellationToken cancellationToken);
        Task<Guid> GenerateRefreshToken(User user, Guid jti, CancellationToken cancellationToken);
        public Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaims(string jwtToken);
    }
}
