using Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Product.Utility
{
    public class Utility
    {
        public UserModel GetLoginUser()
        {
            var userCookie = HttpContext.Current.Request.Cookies["userCookie"];
            var userCookieValue = FormsAuthentication.Decrypt(userCookie.Value);
            var user = GetUserByLoginName(userCookieValue.Name);
            return user;
        }

        public UserModel GetUserByLoginName(String loginName)
        {
            EntityContext db = new EntityContext();
            var user = db.Users.Where(u => u.LoginName.Equals(loginName)).Select(u => u).FirstOrDefault();
            return user;
        }
    }
}