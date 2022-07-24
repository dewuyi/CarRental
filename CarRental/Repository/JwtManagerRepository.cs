using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRental.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarRental.Repository;

public class JwtManagerRepository:IJwtManagerRepository
{
    private DbContextOptions<CarContext> _dbContextOptions;

    private readonly IConfiguration _iConfiguration;
    public JwtManagerRepository(IConfiguration iConfiguration, DbContextOptions<CarContext> dbContextOptions)
    {
        _iConfiguration = iConfiguration;
        _dbContextOptions = dbContextOptions;
    }
    public Tokens Authenticate(User user)
    {
        using var dbContext = new CarContext(_dbContextOptions);
        var usersRecords = dbContext.Users;
        if (!usersRecords.Any(u => u.Name == user.Name && u.Password == user.Password)) {
            return null;
        }

        // Else we generate JSON Web Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_iConfiguration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Name)                    
            }),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new Tokens { Token = tokenHandler.WriteToken(token) };

    }
}