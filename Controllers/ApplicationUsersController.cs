using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tider.Models;

namespace Tider.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();


        // GET: ApplicationUsers
        public ActionResult Index() {
            return View(db.Users.ToList());
        }

        // Get: ApplicationUsers/GetAdmin/secretCode
        public ActionResult GetAdmin(string id) {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (id == "mySecretPassHehe") {
                UserManager.AddToRole(User.Identity.GetUserId(), Const.ADMIN);
                UserManager.AddToRole(User.Identity.GetUserId(), Const.MODERATOR);
            }
            return Redirect("Index");
        }

        // GET: ApplicationUsers/ProfilePage/UserID
        public ActionResult ProfilePage(string id) {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProfilePageViewModel viewModel = new ProfilePageViewModel {
                User = db.Users.Find(id),
                Posts = db.Posts.Where(p => p.OpId == id).ToList(),
            };

            viewModel.PostsCount = viewModel.Posts.Count();

            if (viewModel.User == null) return HttpNotFound();

            var userId = User.Identity.GetUserId();
            foreach (var item in db.UserRoles.Where(p => p.UserId == userId).ToList())
                Debug.WriteLine(db.Roles.Find(item.RoleId).Name);


            ViewBag.isAdmin = User.IsInRole(Const.ADMIN);
            ViewBag.isMod = User.IsInRole(Const.MODERATOR);
            ViewBag.UserId = User.Identity.GetUserId();

            return View(viewModel);
        }


        //POST: /ApplicationUsers/EditRole/?role=User&userId=@Model.Id
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole() {
            //var action = Request.QueryString["action"];
            string role = Request.QueryString["role"];
            string userId = Request.QueryString["userId"];

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            UserManager.RemoveFromRole(userId, Const.ADMIN);
            UserManager.RemoveFromRole(userId, Const.MODERATOR);

            if (role == Const.ADMIN) UserManager.AddToRoles(userId, new string[] { Const.ADMIN, Const.MODERATOR });
            else if (role == Const.MODERATOR) UserManager.AddToRole(userId, Const.MODERATOR);
            else if (role != Const.USER) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return Redirect(Request.UrlReferrer.ToString());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditApplicationUserViewModel viewModel){
            try {
                if (ModelState.IsValid) {
                    var applicationUser = db.Users.Find(User.Identity.GetUserId());
                    db.Entry(applicationUser).CurrentValues.SetValues(viewModel);
                    db.SaveChanges();
                    return Redirect(Request.UrlReferrer.ToString());
                }

                //if (ModelState.IsValid) {
                //    db.Entry(applicationUser).State = EntityState.Modified;
                //    db.SaveChanges();
                //    return RedirectToAction("Index");
                //}
                return Redirect(Request.UrlReferrer.ToString());
            } catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors) {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors) {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


            
        }

        // POST: ApplicationUsers/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id) {
            ApplicationUser applicationUser = db.Users.Find(id);

            var postsRemove = db.Posts.Where(p => p.OpId == id);
            var categoriesRemove = db.Categories.Where(p => p.OpId == id);

            foreach (var item in postsRemove) db.Posts.Remove(item);
            foreach (var item in categoriesRemove) db.Categories.Remove(item);

            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
