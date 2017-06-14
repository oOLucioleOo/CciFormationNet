using Entity;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Services
{
    public static class UserService
    {
        public static long GetUsers(string login, string password)
        {
            var UserRepository = new UserRepository();

            var user = UserRepository.GetUserByLoginPwd(login, password);

            try
            {
                if (user.USER_ID == 0)
                {
                    return 0;
                }
                else
                {
                    return user.USER_ID;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
