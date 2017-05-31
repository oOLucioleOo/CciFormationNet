using Entity;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class UserController : ApiController
    {
        [Route("api/user/GetUsers/")]
        [HttpPost]
        public long GetUsers(string login, string password)
        {
            return UserService.GetUsers(login, password);
        }

        [HttpGet]
        public string getuser()
        {
            Debug.WriteLine("coucou");
            return "coucou";
        }
    }
}
