using Microsoft.IdentityModel.Tokens;
using PetFamily.Core.Options;
using System.Text;

namespace PetFamily.Accounts.Infrastructure.Authorization
{
    public static class TokenValidationParametersFactory
    {
        public static TokenValidationParameters CreateTokenValidation(JwtOptions jwtOptions, bool validateLifetime) =>
            new()
            {
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = validateLifetime,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };

        public static TokenValidationParameters CreateWithoutLifetime(JwtOptions jwtOptions) =>
             CreateTokenValidation(jwtOptions, false);


        public static TokenValidationParameters CreateWithLifetime(JwtOptions jwtOptions) =>
            CreateTokenValidation(jwtOptions, true);
    }
}
