namespace Product.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EntityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EntityContext context)
        {
            UserModel User = new UserModel();
            User.Role = Role.SuperAdmin;
            User.Id = Guid.NewGuid();
            User.LoginName = "Super";
            User.PassWord = "123456";
            var isExist = context.Users.Where(u => u.LoginName.Equals("Super")).Select(u => u).ToList();
            if (isExist.Count() == 0)
            {
                context.Users.Add(User);
            }
        }
    }
}
