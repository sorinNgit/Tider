using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tider.Models;

namespace Tider.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index(int? postId) {
            if (postId == null) return View("Error");
            TempData["postId"] = postId;

            List<Comment> comments = new List<Comment>();

            comments = db.Comments.Include(c => c.Op).Include(c => c.Post).Where(c => c.PostId == postId).ToList();

            CommentsViewModel commentsViewModel = new CommentsViewModel {
                UserImage = User.Identity.IsAuthenticated ? db.Users.Find(User.Identity.GetUserId()).Image_url : null,
                IsAdmin = User.IsInRole(Const.ADMIN),
                IsMod = User.IsInRole(Const.MODERATOR),
                IsUser = User.Identity.IsAuthenticated,
                PostId = postId,
                Comments = comments,
                Post = db.Posts.Find(postId)
            };

            //TODO: ask how do I deal with partials and ViewModels data aka without using ViewBag
            ViewBag.IsAdmin = User.IsInRole(Const.ADMIN);
            ViewBag.IsMod = User.IsInRole(Const.MODERATOR);
            ViewBag.UserID = User.Identity.GetUserId();

            return View(commentsViewModel);
        }

        // POST: Comments/Create
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Content,Date,OpId,PostId")] Comment comment) {
            comment.Date = DateTime.Now;
            comment.OpId = User.Identity.GetUserId();

            if (TempData.ContainsKey("postId")) comment.PostId = (int)TempData["postId"];
            else return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid) {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }

            //ViewBag.OpId = new SelectList(db.Users, "Id", "RealName", comment.OpId);
            //ViewBag.PostId = new SelectList(db.Posts, "ID", "Content", comment.PostId);

            return Redirect(Request.UrlReferrer.ToString());
        }

        // POST: Comments/Edit/5
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content,Date,OpId,PostId")] Comment comment) {
            comment.Date = DateTime.Now;
            comment.OpId = User.Identity.GetUserId();

            if (TempData.ContainsKey("postId")) {
                comment.PostId = (int)TempData["postId"];
            } else return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid) {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }

            //ViewBag.OpId = new SelectList(db.Users, "Id", "RealName", comment.OpId);
            //ViewBag.PostId = new SelectList(db.Posts, "ID", "Content", comment.PostId);

            return Redirect(Request.UrlReferrer.ToString());
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Moderator,User")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
