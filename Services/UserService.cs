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

            var response = user.USER_ID;

            try
            {
                return response;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }
      
    }
}
