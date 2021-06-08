using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Tider.Models {
    public class ApplicationUser : IdentityUser {
        public string RealName { get; set; }
        public string NickName { get; set; }
        public string Image_url { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

        //public virtual ICollection<Category> Subscriptions { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false) {
            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<IdentityUserRole> UserRoles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

    }
}