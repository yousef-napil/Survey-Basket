
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Survey_Basket.Authentication;

public class JWTProvider(IOptions<JwtOptions> jwtOptions) : IJWTProvider
{
    private readonly JwtOptions jwtOptions = jwtOptions.Value;

    public (string token, int expiresIn) GenerateToken(ApplicationUser user)
    {
        Claim[] claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName!),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
        var SigningCredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var expiresIn = jwtOptions.ExpireTime;
        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience:jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresIn),
            signingCredentials: SigningCredentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresIn * 60);
    }

    public string? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SymmetricSecurityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        }
        catch
        {
            return null;
        }

    }
}
