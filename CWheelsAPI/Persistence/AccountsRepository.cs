using AuthenticationPlugin;
using CWheelsAPI.Core;
using CWheelsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CWheelsAPI.Persistence
{
    public class AccountsRepository : Repository<User>, IAccountsRepository
    {

        public AccountsRepository(CWheelsDbContext context) : base(context)
        {
        }

        public User FindUserByEmail(string email)
        {
            return CWheelsDbContext.Users.Where(u => u.Email == email).SingleOrDefault();
        }

        public void Register(User user)
        {
            user.Password = SecurePasswordHasherHelper.Hash(user.Password);
            CWheelsDbContext.Users.Add(user);
        }

        public TokenResponse Login(User user, AuthService auth)
        {
            var claims = new[]
           {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Email, user.Email)
            };

           return auth.GenerateAccessToken(claims);

        }

        public CWheelsDbContext CWheelsDbContext
        {
            get
            {
                return context as CWheelsDbContext;
            }
        }
    }
}
