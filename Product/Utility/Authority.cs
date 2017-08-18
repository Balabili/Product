using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Product.Utility
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool result = false;
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            var userCookie = httpContext.Request.Cookies["userCookie"];
            var userCookieValue = FormsAuthentication.Decrypt(userCookie.Value);
            if (userCookieValue.UserData == "Super Admin" || userCookieValue.UserData == "Admin")
            {
                result = true;
            }
            return result;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            filterContext.Result = new RedirectResult("/Product/ProductList");
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
    }
}