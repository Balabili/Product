using log4net;
using Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product.Controllers
{
    public class RegisterController : Controller
    {
        ILog logs = LogManager.GetLogger(typeof(RegisterController));

        private Utility.Utility utility = new Utility.Utility();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RegisterValidation(String loginName)
        {
            try
            {
                EntityContext db = new EntityContext();
                var data = loginName.Trim();
                var user = db.Users.Where(u => u.LoginName.Equals(data)).Select(u => u).ToList();
                Boolean result = user.Count() != 0;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.Error($"An error occurred while Register Validation.Error:{ex}.");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Register(UserModel user)
        {
            try
            {
                EntityContext db = new EntityContext();
                var loginName = user.LoginName;
                var existedUser = db.Users.Where(u => u.LoginName.Equals(loginName)).Select(u => u).ToList();
                Boolean passValidation = existedUser.Count() == 0 ? true : false;
                if (passValidation)
                {
                    user.Id = Guid.NewGuid();
                    db.Users.Add(user);
                    db.SaveChanges();
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logs.Error($"An error occurred while Register Validation.Error:{ex}.");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
