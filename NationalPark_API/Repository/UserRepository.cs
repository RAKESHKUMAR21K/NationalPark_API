using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NationalPark_API.Data;
using NationalPark_API.Models;
using NationalPark_API.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NationalPark_API.Repository
{    
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _Context;
        private readonly AppSetting _AppSetting;
        public UserRepository(ApplicationDbContext context, IOptions<AppSetting> appSetting)
        {
            _Context = context;
            _AppSetting = appSetting.Value;
        }
        public User Authenticate(string UserName, string Password)
        {
            var UserInDb = _Context.Users.FirstOrDefault(x => x.Name == UserName && x.Password == Password);
            if (UserInDb == null) return null;
            //JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_AppSetting.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,UserInDb.Id.ToString()),
                    new Claim(ClaimTypes.Role,UserInDb.Role)
                }),
                Expires=DateTime.UtcNow.AddDays(7),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)               

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            UserInDb.Token = tokenHandler.WriteToken(token);

            //*****
            UserInDb.Password = "";
            return UserInDb;
        }

        public bool IsUniqueUser(string UserName)
        {
            var userInDb = _Context.Users.FirstOrDefault(u=>u.Name== UserName);
            if (userInDb == null) return true;
            return false;
        }

        public User Register(string UserName, string Password)
        {
            User user = new User()
            {
                Name= UserName,
                Password= Password,
                Role="Admin"
            };
            _Context.Users.Add(user);
            _Context.SaveChanges();
            return user;
        }
    }
}
