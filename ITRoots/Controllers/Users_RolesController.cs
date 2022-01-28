using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ITRoots.Models;

namespace ITRoots.Controllers
{
    public class Users_RolesController : BaseController
    {
        private db_a79052_rootdbEntities db = new db_a79052_rootdbEntities();

        // GET: Users_Roles
        public ActionResult Index()
        {
            var users_Roles = db.Users_Roles.Include(u => u.Roles).Include(u => u.Users);
            return View(users_Roles.ToList());
        }

        // GET: Users_Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users_Roles users_Roles = db.Users_Roles.Find(id);
            if (users_Roles == null)
            {
                return HttpNotFound();
            }
            return View(users_Roles);
        }

        // GET: Users_Roles/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.Roles, "ID", "Name");
            ViewBag.UserId = new SelectList(db.Users, "ID", "FullName");
            return View();
        }

        // POST: Users_Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserId,RoleId")] Users_Roles users_Roles)
        {
            if (ModelState.IsValid)
            {
                db.Users_Roles.Add(users_Roles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleId = new SelectList(db.Roles, "ID", "Name", users_Roles.RoleId);
            ViewBag.UserId = new SelectList(db.Users, "ID", "FullName", users_Roles.UserId);
            return View(users_Roles);
        }

        // GET: Users_Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users_Roles users_Roles = db.Users_Roles.Find(id);
            if (users_Roles == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Roles, "ID", "Name", users_Roles.RoleId);
            ViewBag.UserId = new SelectList(db.Users, "ID", "FullName", users_Roles.UserId);
            return View(users_Roles);
        }

        // POST: Users_Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserId,RoleId")] Users_Roles users_Roles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users_Roles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(db.Roles, "ID", "Name", users_Roles.RoleId);
            ViewBag.UserId = new SelectList(db.Users, "ID", "FullName", users_Roles.UserId);
            return View(users_Roles);
        }

        // GET: Users_Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users_Roles users_Roles = db.Users_Roles.Find(id);
            if (users_Roles == null)
            {
                return HttpNotFound();
            }
            return View(users_Roles);
        }

        // POST: Users_Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users_Roles users_Roles = db.Users_Roles.Find(id);
            db.Users_Roles.Remove(users_Roles);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
