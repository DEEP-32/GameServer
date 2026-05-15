using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Server.Data;
using Server.Models;
using SharedLibrary;

namespace Server.Services;

public class AuthenticationService : IAuthenticationService {
    readonly GameDbContext dbContext;
    readonly Settings settings;

    public AuthenticationService(Settings settings,GameDbContext dbContext) {
        this.settings = settings;
        this.dbContext = dbContext;
    }
    
    public (bool success, string context) Register(string username, string password) {
        if (dbContext.Users.Any(u => u.Username == username))
            return (false, "Username not available");
        
        var user = new User() {
            Username = username,
            Password = password
        };
        
        user.ProvideSaltAndHash();
        dbContext.Add(user);
        dbContext.SaveChanges();
        return (true, "User created");
    }
    public (bool success, string context) Login(string username, string password) {
        var user = dbContext.Users.Include(u=>u.Heroes).FirstOrDefault(u => u.Username == username);
        if(user == null) return (false, "User not found");
        
        if(user.Password != AuthenticationHelpers.ComputeHash(password, user.Salt))
            return (false, "Invalid password");
        
        return (true, GenerateJwtToken(AssembleClaimsIdentity(user)));
    }

    private ClaimsIdentity AssembleClaimsIdentity(User user) {
        var subject = new ClaimsIdentity(new[] {
            new Claim("id", user.Id.ToString()),
            new Claim("heroes", JsonConvert.SerializeObject(user.Heroes.Select(h => h.Id)))
        });
        
        return subject;
    }

    private string GenerateJwtToken(ClaimsIdentity subject) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(settings.BearerKey);
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = subject,
            Expires = DateTime.UtcNow.AddYears(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public interface IAuthenticationService {
    (bool success, string context) Register(string username, string password);
    (bool success, string context) Login(string username, string password);
}

public static class AuthenticationHelpers {
    public static void ProvideSaltAndHash(this User user) {
        var salt = GenerateSalt();
        user.Salt = Convert.ToBase64String(salt);
        user.Password = ComputeHash(user.Password, user.Salt);
    }

    private static byte[] GenerateSalt() {
        var rng = RandomNumberGenerator.Create();
        var salt = new byte[24];
        rng.GetBytes(salt);
        return salt;
    }
    
    public static string ComputeHash(string password, string saltString) {
        var salt = Convert.FromBase64String(saltString);

        using var hashGenerator = new Rfc2898DeriveBytes(password, salt);
        hashGenerator.IterationCount = 10101;
        var bytes = hashGenerator.GetBytes(24);
        return Convert.ToBase64String(bytes);
    }
}