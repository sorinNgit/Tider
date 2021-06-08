using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tider.Models;

namespace Tider.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        //readonly UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

        // GET: Categories
        public ActionResult Index() {
            ViewBag.userImage = User.Identity.IsAuthenticated ? db.Users.Find(User.Identity.GetUserId()).Image_url : null;
            ViewBag.isAdmin = User.IsInRole(Const.ADMIN);

            return View(db.Categories.ToList());
        }

        // GET: Categories/Posts/5
        public ActionResult Posts(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null) {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,Image_url")] Category category) {
            category.OpId = User.Identity.GetUserId();
            category.Date = DateTime.Now;

            if (ModelState.IsValid) {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // POST: Categories/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,Image_url")] Category category) {
            category.OpId = User.Identity.GetUserId();
            category.Date = DateTime.Now;

            if (ModelState.IsValid) {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction(Request.UrlReferrer.ToString());
        }

        protected override void Dispose(bool disposing) {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
