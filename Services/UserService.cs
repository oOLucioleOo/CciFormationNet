using Entity;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Services
{
    public static class UserService
    {
        public static long GetUsers(string login, string password)
        {
            var UserRepository = new UserRepository();

            var user = UserRepository.GetUserByLoginPwd(login, password);

            var response = user.USER_ID;

            if (response == 0)
            {
                //MessageBox.Show("Votre login ou votre mot de passe n'est pas correct.",
                //"Erreur d'authentification",
                //MessageBoxButton.OKCancel,
                //MessageBoxImage.Warning);
            }

            return response;
        }
      
    }
}
