using log4net;
using Product.Models;
using System;
using Product.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Product.Controllers
{
    public class ProductController : Controller
    {
        ILog logs = LogManager.GetLogger(typeof(ProductController));

        private Utility.Utility utility = new Utility.Utility();

        public ActionResult Index()
        {
            var user = utility.GetLoginUser();
            return View(user);
        }

        public ActionResult ProductList()
        {
            UserModel user = new UserModel();
            try
            {
                user = utility.GetLoginUser();
                ViewData["UserName"] = user.LoginName;
            }
            catch (Exception ex)
            {
                logs.Error("Error occurred while view ProductList:{0}", ex);
            }
            return View(user);
        }

        public ActionResult AddProduct(String productName, String productDescription)
        {
            try
            {
                EntityContext db = new EntityContext();
                ProductModel product = new ProductModel();
                product.Id = Guid.NewGuid();
                product.Name = productName;
                product.Description = productDescription;
                db.Products.Add(product);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logs.Error("Error occurred while add Product", ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ProductDetail(Guid id)
        {
            try
            {
                var loginUser = utility.GetLoginUser();
                ViewData["UserName"] = loginUser.LoginName;
                EntityContext db = new EntityContext();
                var user = db.Users.Find(loginUser.Id);
                var product = db.Products.Find(id);
                var map = db.Maps.Where(m => m.ProductId.Equals(product.Id) && m.UserId.Equals(user.Id)).FirstOrDefault();
                if (map == null)
                {
                    Map newMap = new Map();
                    newMap.Id = Guid.NewGuid();
                    newMap.ProductId = product.Id;
                    newMap.UserId = user.Id;
                    newMap.LastViewDate = DateTime.Now.ToUniversalTime().Ticks;
                    db.Maps.Add(newMap);
                }
                else
                {
                    map.LastViewDate = DateTime.Now.ToUniversalTime().Ticks; ;
                }
                db.SaveChanges();
                return View(product);
            }
            catch (Exception ex)
            {
                logs.Error("Error occurred while view Product detail", ex);
                return View(new ProductModel());
            }
        }

        [HttpPost]
        public void DeleteProduct(Guid id)
        {
            try
            {
                EntityContext db = new EntityContext();
                var product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                logs.Error("Error occurred while delete Product", ex);
            }
        }

        public void UpdateProduct(Guid id, String productName, String description)
        {
            try
            {
                EntityContext db = new EntityContext();
                var product = db.Products.Find(id);
                product.Name = productName;
                product.Description = description;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                logs.Error("Error occurred while delete Product", ex);
            }
        }

        [HttpGet]
        public JsonResult GetProducts()
        {
            EntityContext db = new EntityContext();
            var productList = db.Products.ToList();
            var total = productList.Count();
            return Json(new { total = total, rows = productList }, JsonRequestBehavior.AllowGet);
        }
    }
}
