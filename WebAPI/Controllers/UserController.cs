using Entity;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class UserController : ApiController
    {
        public USERS GetUsers(long login, string password)
        {
            return UserService.GetUsers(login, password);
        }
    }
}
