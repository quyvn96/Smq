namespace Smq.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Smq.Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Smq.Data.SmqSolutionDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Smq.Data.SmqSolutionDbContext context)
        {
            CreateProductCategorySample(context);
            CreatePage(context);
            //  This method will be called after migrating to the latest version.

            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new SmqSolutionDbContext()));

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new SmqSolutionDbContext()));

            //var user = new ApplicationUser()
            //{
            //    UserName = "quyvn96",
            //    Email = "quyvn317@gmail.com",
            //    EmailConfirmed = true,
            //    BirthDay = DateTime.Now,
            //    FullName = "Technology Education"
            //};

            //manager.Create(user, "123654$");

            //if (!roleManager.Roles.Any())
            //{
            //    roleManager.Create(new IdentityRole { Name = "Admin" });
            //    roleManager.Create(new IdentityRole { Name = "User" });
            //}

            //var adminUser = manager.FindByEmail("quyvn317@gmail.com");

            //manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
        }
        private void CreateProductCategorySample(Smq.Data.SmqSolutionDbContext context)
        {
            if (context.ProductCategories.Count() == 0)
            {
                List<ProductCategory> listProductCategory = new List<ProductCategory>
            {
                new ProductCategory(){Name="Điện lạnh",Alias="DL01",Status=true},
                new ProductCategory(){Name="Viễn thông",Alias="VT01",Status=true},
                new ProductCategory(){Name="Đồ gia dụng",Alias="GD01",Status=true},
                new ProductCategory(){Name="Mỹ phẩm",Alias="MP01",Status=true},
            };
                context.ProductCategories.AddRange(listProductCategory);
                context.SaveChanges();
            }
        }
        private void CreatePage(SmqSolutionDbContext context)
        {
            if (context.Pages.Count() == 0)
            {
                var page = new Page()
                {
                    Name = "Giới thiệu",
                    Alias="gioi-thieu",
                    Content = @"Techopedia explains Product Introduction Product introduction is primarily a marketing approach and is a planned initiative to make a product available to specific market segments. 
                                Depending on the nature of the product, market dynamics, regulatory requirements and other economic and social constraints, the product introduction process can vary.
                                For example, a software development company may introduce a new product through a beta version before launching the complete solution. 
                                This allows the company to gain valuable user feedback, and to fix bugs and errors before the complete launch.",
                    Status = true
                };
                context.Pages.Add(page);
                context.SaveChanges();
            }
        }
    }
}