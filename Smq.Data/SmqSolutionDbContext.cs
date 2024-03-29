﻿using Microsoft.AspNet.Identity.EntityFramework;
using Smq.Model.Models;
using System.Data.Entity;

namespace Smq.Data
{
    public class SmqSolutionDbContext : IdentityDbContext<ApplicationUser>
    {
        public SmqSolutionDbContext()
            : base("SmqShopConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Footer> Footers { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuGroup> MenuGroups { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<SupportOnline> SupportOnlines { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<VisitorStatistic> VisitorStatistics { get; set; }
        public DbSet<Error> Errors { get; set; }

        public DbSet<ContactDetail> ContactDetails { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<ApplicationGroup> ApplicationGroups { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationRoleGroup> ApplicationRoleGroups { get; set; }
        public DbSet<ApplicationUserGroup> ApplicationUserGroups { get; set; }

        public static SmqSolutionDbContext Create()
        {
            return new SmqSolutionDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {

            builder.Entity<IdentityUserRole>().HasKey(n => new { n.UserId, n.RoleId }).ToTable("ApplicationUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(n => n.UserId).ToTable("ApplicationUserLogins");
            builder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            builder.Entity<IdentityUserClaim>().HasKey(n => n.UserId).ToTable("ApplicationUserClaims");
        }
    }
}