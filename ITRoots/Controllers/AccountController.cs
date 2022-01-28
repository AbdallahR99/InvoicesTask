using AutoMapper;
using ITRoots.Models;
using ITRoots.Models.AccountVM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ITRoots.Controllers
{
    public class AccountController : BaseController
    {
        private db_a79052_rootdbEntities _context;

        private readonly IMapper mapper;
        public AccountController()
        {
            _context = new db_a79052_rootdbEntities();
            mapper = AutoMapperConfig.Mapper;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            InitializaUserType();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM loginVM)
        {

            InitializaUserType();
            if (ModelState.IsValid)
            {
                var user = _context.LoginUser(loginVM.Username, EncodeToBase64.EncodePasswordToBase64( loginVM.Password)).FirstOrDefault();
                if(user == null)
                {
                    ModelState.AddModelError("", Resources.Resource.InvalidLogin );
                    return View(loginVM);
                }

                if (!user.IsEmailConfirmed)
                {
                    ModelState.AddModelError("NotActivated", "Please Verify your email first, <a href=''>click here</a> to verify your account");
                    TempData["Email"] = user.Email;
                    return View(loginVM);
                }
                UserVM userVM = mapper.Map<UserVM>(user);
                userVM.UserType = loginVM.UserType;
                if (userVM.UserType == Constants.AdminsRole)
                    ManageUserRole(user.ID, true);
                else
                    ManageUserRole(user.ID, false);
                Session[Constants.LoggedInUserSessionKey] = userVM;
                return RedirectToAction("Index","Users");
            }
            
            return View(loginVM);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                var isUserExist = _context.Users.Any(a=>a.Username == registerVM.Username);
                if (isUserExist)
                {
                    ModelState.AddModelError("", "Username already exist ");
                    return View(registerVM);
                }

                var isEmailExist = _context.Users.Any(a => a.Email == registerVM.Email);
                if (isEmailExist)
                {
                    ModelState.AddModelError("", "Email already exist ");
                    return View(registerVM);
                }

                var isPhoneNumberExist = _context.Users.Any(a => a.PhoneNumber == registerVM.PhoneNumber);
                if (isPhoneNumberExist)
                {
                    ModelState.AddModelError("", "PhoneNumber already exist ");
                    return View(registerVM);
                }

                Users user = mapper.Map<RegisterVM, Users>(registerVM);
                user.CreatedDate = DateTime.Now;
                bool isFirstUser = _context.Users.Count() == 0;
                user.ActivationCode = new Random().Next(1111, 99999).ToString();
                user.Password = EncodeToBase64.EncodePasswordToBase64(user.Password);

               _context.Users.Add(user);

                if (isFirstUser)
                {
                    Roles AdminRole = _context.Roles.FirstOrDefault(x => x.Name == Constants.AdminsRole);
                    user.Users_Roles.Add(new Users_Roles()
                    {
                        RoleId = AdminRole.ID,
                        UserId = user.ID
                    });

                }
                _context.SaveChanges();

                TempData["Email"] = user.Email;
                return RedirectToAction("ConfirmEmail");
            }

            return View(registerVM);
        }

        public ActionResult ConfirmEmail()
        {
            ConfirmEmailVM viewM = new ConfirmEmailVM()
            {
                Email = TempData["Email"].ToString()
            };
            var user = _context.Users.FirstOrDefault(a => a.Email == viewM.Email);
            if (user == null)
                return HttpNotFound();

            SendEmail(user.Email, user.ActivationCode);

            return View(viewM);
        }

        [HttpPost]
        public ActionResult ConfirmEmail(ConfirmEmailVM confirmEmailVM)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(a => a.Email == confirmEmailVM.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Email not found ");
                    return View(confirmEmailVM);
                }

                if (user.ActivationCode != confirmEmailVM.Code)
                {
                    ModelState.AddModelError("", Resources.Resource.InvalidCode);
                    return View(confirmEmailVM);
                }

                user.IsEmailConfirmed = true;
                _context.SaveUser(user.ID, user.FullName, user.Username,
                user.Password, user.Email, user.PhoneNumber, user.IsEmailConfirmed, false);

                _context.SaveChanges();
                Session[Constants.LoggedInUserSessionKey] = mapper.Map<UserVM>(user);
                return RedirectToAction("Index", "Users");

            }
                return View(confirmEmailVM);
        }


        public ActionResult Logout()
        {
            Session[Constants.LoggedInUserSessionKey] = null;

            return RedirectToAction("Index", "Users");
        }

        public ActionResult UnAuthorized()
        {

            return View();
        }

        private void InitializaUserType()
        {
            var listItems = new List<ListItem> {
             new ListItem { Text = Resources.Resource.User, Value = "User" },
             new ListItem { Text = Resources.Resource.Admin, Value = "Admin" },

        };
            ViewBag.UserType = new SelectList(listItems);
        }

        private void ManageUserRole(int userID,bool isAdd)
        {
            Roles AdminRole = _context.Roles.FirstOrDefault(x => x.Name == Constants.AdminsRole);
            Users user = _context.Users.Find(userID);
            if (isAdd)
            {
                _context.Users_Roles.Add(new Users_Roles()
                {
                    RoleId = AdminRole.ID,
                    UserId = userID
                });
            }
            else
            {
                var userRoles = user.Users_Roles.FirstOrDefault(a => a.RoleId == AdminRole.ID);
                if(userRoles != null)
                {
                    _context.Users_Roles.Remove(userRoles);
                }
            }
          
              _context.SaveChanges();

        }

        private void SendEmail(string UserEmail, string Code)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(UserEmail);
            mail.From = new MailAddress(ConfigurationManager.AppSettings["SenderEmail"]);
            mail.Subject = Resources.Resource.VerifyEmail;
            string Body = Resources.Resource.VerifyEmailBody + "\n " + Code;

            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = 
                new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SenderEmail"],
                        ConfigurationManager.AppSettings["SenderPassword"]);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

    }
}