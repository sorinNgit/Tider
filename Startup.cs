using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Tider.Models;

[assembly: OwinStartupAttribute(typeof(Tider.Startup))]
namespace Tider
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Calling the method which is creating the application roles
            CreateApplicationRoles();

        }

        private void CreateApplicationRoles() {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists(Const.ADMIN)) {
                IdentityRole role = new IdentityRole {
                    Name = Const.ADMIN
                };
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists(Const.MODERATOR)) {
                IdentityRole role = new IdentityRole {
                    Name = Const.MODERATOR
                };
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists(Const.USER)) {
                IdentityRole role = new IdentityRole {
                    Name = Const.USER
                };
                roleManager.Create(role);
            }
        }
    }    
}
