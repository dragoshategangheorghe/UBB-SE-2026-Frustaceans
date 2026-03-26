using PharmacyApp.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Accounts.Logic
{
    public static class ServiceWrapper
    {
        public static UserAccountService UserAccountService { get; private set; }

        public static void Initialize()
        {
            IUsersRepository userRepo = new SQLUsersRepository();
            UserAccountService = new UserAccountService(userRepo);
        }
    }
}
