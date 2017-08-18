using Product.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Product.Migrations;
using System.Data.Entity.Migrations;
using System.IO;

namespace Product
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //using (var db = new EntityContext())
            //{
            //    db.Database.Initialize(false);
            //}
            //Database.SetInitializer(new DbInitializer());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EntityContext, Configuration>());
            var dbMigrator = new DbMigrator(new Configuration());
            dbMigrator.Update();

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("Log4Net.config")));

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}