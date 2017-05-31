using Entity;
using Services;
using System.Diagnostics;
using System.Web.Http;
using System.Windows;

namespace WebAPI.Controllers
{
    public class UserController : ApiController
    {
        [Route("api/user/GetUsers/")]
        [System.Web.Http.HttpPost]
        public long GetUsers([FromBody]USER user)
        {        
            return UserService.GetUsers(user.USER_LOG, user.USER_PWD);
        }
    }
}