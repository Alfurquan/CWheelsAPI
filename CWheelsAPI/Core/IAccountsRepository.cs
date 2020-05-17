using AuthenticationPlugin;
using CWheelsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Core
{
    public interface IAccountsRepository : IRepository<User>
    {
        User FindUserByEmail(string email);

        void Register(User user);

        TokenResponse Login(User user,AuthService auth);

    }
}
