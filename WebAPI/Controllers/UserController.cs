using Entity;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

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