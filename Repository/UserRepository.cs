//using Entity;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Repository
//{
//    class UserRepository : GenericRepository<CciFormationNetEntities, USERS>
//    {
//        // Get for USER
//        public USERS GetUserById(int UserId)
//        {
//            var query = GetAll().SingleOrDefault(x => x.USER_ID == UserId);
//            return query;
//        }

//        public USERS GetUserByLoginPwd(long UserLogin, String UserPwd)
//        {
//            var query = GetAll().SingleOrDefault(x => x.USER_LOG == UserLogin && x.USER_PWD == UserPwd);
//            return query;
//        }

//        public USERS GetUserByMail(String UserMail)
//        {
//            var query = GetAll().SingleOrDefault(x => x.USER_MAIL == UserMail);
//            return query;
//        }

//        // Add USER
//        public void AddUser(long UserLogin, String UserPwd, String UserFirstName, String UserLastname, String UserMail)
//        {
//            USERS user = new USERS();
//            user.USER_LOG = UserLogin;
//            user.USER_PWD = UserPwd;
//            user.USER_FIRSTNAME = UserFirstName;
//            user.USER_LASTNAME = UserLastname;
//            user.USER_MAIL = UserMail;
//            Add(user);
//            Context.SaveChanges();
//        }

//        // delete USER by ID
//        public void DeleteUserById(int UserId)
//        {
//            var user = FindBy(x => x.USER_ID == UserId).SingleOrDefault();
//            Delete(user);
//            Context.SaveChanges();
//        }

//        // delete USER by LastName
//        public void DeleteUserByLastName(String UserLastName)
//        {
//            var user = FindBy(x => x.USER_LASTNAME == UserLastName).SingleOrDefault();
//            Delete(user);
//            Context.SaveChanges();
//        }

//        // Update USER by ID
//        public void UpdateUserById(long UserLogin, String UserPwd, String UserFirstName, String UserLastname, String UserMail)
//        {
//            var user = FindBy(x => x.USER_MAIL == UserMail).SingleOrDefault();
//            user.USER_LOG = UserLogin;
//            user.USER_PWD = UserPwd;
//            user.USER_FIRSTNAME = UserFirstName;
//            user.USER_LASTNAME = UserLastname;
//            user.USER_MAIL = UserMail;
//            Edit(user);
//            Context.SaveChanges();
//        }
//    }
//}
