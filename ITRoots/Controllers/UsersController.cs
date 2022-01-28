using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using ITRoots.Helpers;
using ITRoots.Models;

namespace ITRoots.Controllers
{
    [CustomAuthorize(Constants.AdminsRole)]
    public class UsersController : BaseController
    {
        private db_a79052_rootdbEntities _context;

        private readonly IMapper mapper;
      
        public UsersController()
        {
            _context = new db_a79052_rootdbEntities();
            mapper = AutoMapperConfig.Mapper;
        }

        // GET: Users
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.IsAdmin = LoggedInAsAdmin;
            return View(mapper.Map<List<UserVM>>(_context.SelectUserList().ToList()));
        }

      
        // GET: Users/Create
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                Users user = mapper.Map<Users>(userVM);
                user.CreatedDate = DateTime.Now;
                user.ActivationCode = new Random().Next(1111, 99999).ToString();
                user.Password = EncodeToBase64.EncodePasswordToBase64(user.Password);

                _context.SaveUser(userVM.ID, userVM.FullName, userVM.Username,
                    userVM.Password, userVM.Email, userVM.PhoneNumber, userVM.IsEmailConfirmed, true);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return PartialView("_Create", userVM);
        }

        
        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Users user = _context.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            UserVM userVM = mapper.Map<UserVM>(user);
            
            return PartialView("_Edit", userVM);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserVM userVM)
        {
      
            if (ModelState.IsValid)
            {
                _context.SaveUser(userVM.ID, userVM.FullName, userVM.Username,
                   userVM.Password, userVM.Email, userVM.PhoneNumber, userVM.IsEmailConfirmed, false);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView("_Edit",userVM);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Users user = _context.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            UserVM userVM = mapper.Map<UserVM>(user);
            return PartialView("_Delete", userVM);
           
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SelectUserList_Result user = _context.SelectUserList().FirstOrDefault(a=>a.ID == id);
            if (user == null)
                return HttpNotFound();

            _context.DeleteUser(id);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
