using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LifeMall.Models;
using LifeMall.ViewModel;
using Newtonsoft.Json;

namespace LifeMall.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            { return View(loginViewModel); }
            var db = new LifeMallDBContext();
            var user = db.Member.Where(m => m.Account == loginViewModel.Account && m.Password == loginViewModel.Password).FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError("", "無效的帳號或密碼。");
                return View();
            }
            var userData = new User()
            {
                Name = user.Name,
                MemberID = user.MemberID,
                identity = (Identity)user.role
            };
            var ticket = new FormsAuthenticationTicket
                (
                version: 1,
                name: user.Name.ToString(), //可以放使用者Id
                issueDate: DateTime.UtcNow,//現在UTC時間
                expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
                isPersistent: true,// 是否要記住我 true or false
                userData: JsonConvert.SerializeObject(userData), //可以放使用者角色名稱
                cookiePath: FormsAuthentication.FormsCookiePath
                );

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");


           



            
        }
        public ActionResult Login()
        {
            return View();
        }



        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();

            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Member member)
        {
            var selectList = new List<SelectListItem>()
                {
                    new SelectListItem {Text="男",Value="value-1"},
                    new SelectListItem {Text="女",Value="value-2"},

                };
            selectList.Where(m => m.Value == member.Sex).First().Selected = true;
            ViewBag.SelectList = selectList;
            if (ModelState.IsValid == true)
            {
                member.Sex = selectList.Where(m => m.Value == member.Sex).FirstOrDefault().Text;
                using (var db = new LifeMallDBContext())
                {
                    var newMember = new Member()
                    {
                        Name = member.Name,
                        Account = member.Account,
                        Password = member.Password,
                        Address = member.Address,
                        Email = member.Email,
                        Phone = member.Phone,
                        Sex = member.Sex,
                        Birthday = member.Birthday,
                        CreateDate = DateTime.Now
                        

                    };
                    db.Member.Add(newMember);
                    db.SaveChanges();

                }




                return RedirectToAction("Index", "Home");
            }
            
            return View(member);
        }

        public ActionResult Register()
        {
            var selectList = new List<SelectListItem>()
            {
                new SelectListItem {Text="男",Value="value-1"},
                new SelectListItem {Text="女",Value="value-2"},

            };
            
            ViewBag.SelectList = selectList;
            return View();
        }

        public enum Identity
        {
            Admin=1,
            User=2
        }

        public class User
        {
            public string Name { get; set; }
            public int MemberID { get; set; }
            public Identity identity { get; set; }



        }

        public bool IsLogin()
        {
            var user = GetUser();
            if(user==null)
            {
                return false;
            }
            return true;
        }
        public User GetUser()
        {
            var user = System.Web.HttpContext.Current.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var identity = (FormsIdentity)user.Identity;
                var ticket = identity.Ticket;
                return JsonConvert.DeserializeObject<User>(ticket.UserData);

            }

            return null;
        }

    }
}