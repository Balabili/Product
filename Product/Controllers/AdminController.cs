using log4net;
using Product.Models;
using Product.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Product.Controllers
{
    public class AdminController : Controller
    {
        ILog logs = LogManager.GetLogger(typeof(AdminController));

        private Utility.Utility utility = new Utility.Utility();

        public ActionResult Index()
        {
            EntityContext db = new EntityContext();
            db.Users.ToList();
            db.UserInformations.ToList();
            db.Products.ToList();
            db.Maps.ToList();
            return View("~/Views/Register/Index.cshtml");
        }

        [CustomAuthorize]
        public ActionResult UserList()
        {
            var loginUser = utility.GetLoginUser();
            ViewData["UserName"] = loginUser.LoginName;
            return View(loginUser);
        }

        [HttpPost]
        public JsonResult DeleteUser(String userId)
        {
            EntityContext db = new EntityContext();
            var userGuid = new Guid(userId);
            var deleteUser = db.Users.Where(u => u.Id.Equals(userGuid)).Select(u => u).FirstOrDefault();
            if (deleteUser != null)
            {
                db.Users.Remove(deleteUser);
                db.SaveChanges();
            }
            var loginName = deleteUser.LoginName;
            return Json(loginName, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddUser(String loginName, String password, Int32 role)
        {
            try
            {
                EntityContext db = new EntityContext();
                var isExistsUser = db.Users.Where(u => u.LoginName.Equals(loginName)).Select(u => u).Count() != 0;
                if (isExistsUser)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                UserModel user = new UserModel();
                user.Id = Guid.NewGuid();
                user.LoginName = loginName;
                user.PassWord = password;
                user.Role = (Role)role;
                db.Users.Add(user);
                db.SaveChanges();
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.Error("Error occurred while AddUser:{0}", ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateUser(String loginName, String password, Int32 role)
        {
            try
            {
                EntityContext db = new EntityContext();
                var updateUser = db.Users.Where(u => u.LoginName.Equals(loginName)).Select(u => u).FirstOrDefault();
                if (updateUser == null)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                updateUser.LoginName = loginName;
                updateUser.PassWord = password;
                updateUser.Role = (Role)role;
                db.Entry(updateUser).State = EntityState.Modified;
                db.SaveChanges();
                return Json(updateUser, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.Error("Error occurred while Update User:{0}", ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsers()
        {
            EntityContext db = new EntityContext();
            var userList = db.Users.ToList();
            var total = userList.Count();
            return Json(new { total = total, rows = userList }, JsonRequestBehavior.AllowGet);
        }
    }
}
