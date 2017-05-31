using Entity;
using Services;
using System.Diagnostics;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class UserController : ApiController
    {
        [Route("api/user/GetUsers/")]
        [System.Web.Http.HttpPost]
        //public long GetUsers(string login, string password)
        //{
        //    return UserService.GetUsers(login, password);
        //}
        public long GetUsers([FromBody] USER user)
        {        
            return UserService.GetUsers(user.USER_LOG, user.USER_PWD);
        }

        [System.Web.Http.HttpGet]
        public string getuser()
        {
            Debug.WriteLine("coucou");
            return "coucou";
        }
    }
}