using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AuthorizationService.Entities;
using AuthorizationService.Helpers;

namespace AuthorizationService.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {
        private List<User> _users = new List<User>
        {
            // authentication successful so generate jwt token
            new User { accountId =1, userName = "Evan", password="Evanpassword8" },
            new User { accountId =2, userName = "Dwin", password="letme0ut" },
            new User { accountId =3, userName = "Ron", password="123123123" },
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public User Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.userName == username && x.password == password);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.accountId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.InvalidationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                                                        SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.token = tokenHandler.WriteToken(token);

            user.password = null;
            return user;
        }
    }
}