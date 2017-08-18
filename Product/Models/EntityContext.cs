using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Product.Models
{
    public class EntityContext : DbContext
    {
        public EntityContext() : base("myProduct")
        {

        }
        public DbSet<UserModel> Users { get; set; }

        public DbSet<Map> Maps { get; set; }

        public DbSet<ProductModel> Products { get; set; }

        public DbSet<UserInformationModel> UserInformations { get; set; }

        //EntityContext db = new EntityContext();


        //db.SaveChanges();

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<UserModel>().HasOptional(e => e.UserInformation).WithMany().HasForeignKey(m => m.UserInformationId).WillCascadeOnDelete(false);
        //    //modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}