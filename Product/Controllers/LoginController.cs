using log4net;
using Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Security;

namespace Product.Controllers
{
    public class LoginController : Controller
    {
        ILog logs = LogManager.GetLogger(typeof(LoginController));

        private Utility.Utility utility = new Utility.Utility();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(String loginName, String password)
        {
            try
            {
                EntityContext db = new EntityContext();
                var hasUser = db.Users.Where(u => u.LoginName.Equals(loginName)).Select(u => u).ToList();
                if (hasUser == null || (hasUser == null && hasUser.Count() != 0))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (hasUser[0].PassWord != password)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                AddLoginCache(hasUser[0]);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.Error("Error occurred while Login:{0}", ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Logout()
        {
            var userCookie = System.Web.HttpContext.Current.Response.Cookies["userCookie"];
            if (userCookie != null)
            {
                userCookie.Expires = DateTime.Now.AddDays(-1);
            }
            return new RedirectResult("/Login/Index");
        }

        private void AddLoginCache(UserModel user)
        {
            var userRole = "";
            if (user.Role == Role.SuperAdmin)
            {
                userRole = "Super Admin";
            }
            else if (user.Role == Role.Admin)
            {
                userRole = "Admin";
            }
            else
            {
                userRole = "End User";
            }
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.LoginName, DateTime.Now, DateTime.Now.AddMinutes(30), true, userRole);
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie authCookie = new HttpCookie("userCookie", encryptedTicket);
            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
        }
    }
}
