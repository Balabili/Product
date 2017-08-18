using log4net;
using Product.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Product.Controllers
{
    public class UserController : Controller
    {
        ILog logs = LogManager.GetLogger(typeof(UserController));

        private Utility.Utility utility = new Utility.Utility();

        public ActionResult Index()
        {
            try
            {
                var loginUser = utility.GetLoginUser();
                ViewData["UserName"] = loginUser.LoginName;
                return View(loginUser);
            }
            catch (Exception ex)
            {
                logs.Error($"An error occurred while enter user index page.Error:{ex}.");
                return View(new UserModel());
            }
        }

        public ActionResult InformationConfig()
        {
            try
            {
                var loginUser = utility.GetLoginUser();
                ViewData["UserName"] = loginUser.LoginName;
                var userInfo = new UserInformationModel();
                if (loginUser.UserInformationId != null)
                {
                    EntityContext db = new EntityContext();
                    userInfo = db.UserInformations.Find(loginUser.UserInformationId);
                }
                return View(userInfo);
            }
            catch (Exception ex)
            {
                logs.Error($"An error occurred while enter information configuration page.Error:{ex}.");
                return View(new UserInformationModel());
            }
        }

        public ActionResult ChangePassword()
        {
            try
            {
                var loginUser = utility.GetLoginUser();
                ViewData["UserName"] = loginUser.LoginName;
                return View(loginUser);
            }
            catch (Exception ex)
            {
                logs.Error($"An error occurred while enter change password page.Error:{ex}.");
                return View(new UserModel());
            }

        }

        public ActionResult ViewProductDetails(Guid id)
        {
            try
            {
                EntityContext db = new EntityContext();
                var user = db.Users.Find(id);
                ViewData["UserName"] = user.LoginName;
                return View(user);
            }
            catch (Exception ex)
            {
                logs.Error($"An error occurred while enter view product details page.Error:{ex}.");
                return View(new UserModel());
            }
        }

        [HttpGet]
        public JsonResult ViewProductRecord(Guid id)
        {
            try
            {
                EntityContext db = new EntityContext();
                var user = db.Users.Find(id);
                List<ProductItem> productList = new List<ProductItem>();
                var mapList = db.Maps.Where(m => m.UserId.Equals(user.Id)).ToList();
                foreach (var item in mapList)
                {
                    var product = db.Products.Find(item.ProductId);
                    ProductItem productItem = new ProductItem();
                    productItem.Id = product.Id;
                    productItem.Name = product.Name;
                    productItem.Description = product.Description;
                    productItem.LastViewDate = new DateTime(item.LastViewDate).ToLocalTime();
                    productList.Add(productItem);
                }
                var total = productList.Count();
                return Json(new { total = total, rows = productList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.Error($"An error occurred while enter view product records page.Error:{ex}.");
                return Json(new { total = 0, rows = new List<ProductItem>() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public void UpdatePassword(String password, Guid userId)
        {
            try
            {
                EntityContext db = new EntityContext();
                var user = db.Users.Find(userId);
                user.PassWord = password;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                logs.Error($"An error occurred while UpdatePassword.Error:{ex}.");
            }
        }

        [HttpPost]
        public JsonResult SaveInformation(UserInformationModel userInfo)
        {
            try
            {
                var loginUser = utility.GetLoginUser();
                EntityContext db = new EntityContext();
                if (loginUser.UserInformationId == null)
                {
                    userInfo.Id = Guid.NewGuid();
                    db.UserInformations.Add(userInfo);
                    var user = db.Users.Find(loginUser.Id);
                    user.UserInformationId = userInfo.Id;
                }
                else
                {
                    var UserInfoId = loginUser.UserInformationId;
                    var userInfoItem = db.UserInformations.Find(UserInfoId);
                    userInfoItem.Name = userInfo.Name;
                    userInfoItem.Age = userInfo.Age;
                    userInfoItem.Gender = userInfo.Gender;
                    userInfoItem.Email = userInfo.Email;
                }
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.Error($"An error occurred while SaveInformation.Error:{ex}.");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
