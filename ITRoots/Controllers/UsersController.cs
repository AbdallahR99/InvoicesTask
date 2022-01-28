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
                var isUserExist = _context.Users.Any(a => a.Username == userVM.Username);
                if (isUserExist)
                {
                    ModelState.AddModelError("", Resources.Resource.UsernameExist);
                    return View(userVM);
                }

                var isEmailExist = _context.Users.Any(a => a.Email == userVM.Email);
                if (isEmailExist)
                {
                    ModelState.AddModelError("", Resources.Resource.EmailExist);
                    return View(userVM);
                }

                var isPhoneNumberExist = _context.Users.Any(a => a.PhoneNumber == userVM.PhoneNumber);
                if (isPhoneNumberExist)
                {
                    ModelState.AddModelError("", Resources.Resource.PhoneNumberExist);
                    return View(userVM);
                }
                Users user = mapper.Map<Users>(userVM);
                user.CreatedDate = DateTime.Now;
                user.ActivationCode = new Random().Next(1111, 99999).ToString();

                _context.SaveUser(userVM.ID, userVM.FullName, userVM.Username,
                    EncodeToBase64.EncodePasswordToBase64(userVM.Password), userVM.Email, userVM.PhoneNumber, userVM.IsEmailConfirmed, true);
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
                var isUserExist = _context.Users.Any(a => a.Username == userVM.Username);
                if (isUserExist)
                {
                    ModelState.AddModelError("", Resources.Resource.UsernameExist);
                    return View(userVM);
                }

                var isEmailExist = _context.Users.Any(a => a.Email == userVM.Email);
                if (isEmailExist)
                {
                    ModelState.AddModelError("", Resources.Resource.EmailExist);
                    return View(userVM);
                }

                var isPhoneNumberExist = _context.Users.Any(a => a.PhoneNumber == userVM.PhoneNumber);
                if (isPhoneNumberExist)
                {
                    ModelState.AddModelError("", Resources.Resource.PhoneNumberExist);
                    return View(userVM);
                }
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

            DeleteChildren(id);
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

        private void DeleteChildren(int userId)
        {
            List<Invoices> userInvoices = _context.Invoices.Where(a => a.UserID == userId).ToList();
            foreach (var invoice in userInvoices)
            {
                _context.Products.RemoveRange(invoice.Products);
                _context.Invoices.Remove(invoice);
            }
            List<Users_Roles> userRoles = _context.Users_Roles.Where(a => a.UserId == userId).ToList();
            _context.Users_Roles.RemoveRange(userRoles);
            _context.SaveChanges();
        }
    }
    
}
