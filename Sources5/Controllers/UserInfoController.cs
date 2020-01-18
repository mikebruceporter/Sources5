using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Sources5.Data;
using Sources5.Models;
using Microsoft.AspNetCore.Authorization;
namespace Sources5.Controllers
{
    [Authorize(Roles = "Admin,Users")]
    public class UserInfoController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private ApplicationDbContext _context;

        public UserInfoController(ApplicationDbContext context, UserManager<ApplicationUser> usrMgr)
        {
            _context = context;
            userManager = usrMgr;
        }
        [HttpGet]
        public ActionResult Index()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            bool IsAdmin = currentUser.IsInRole("Admin");  // might use this in future
            var Uid = userManager.GetUserId(User); // Get user id:
            UserInfo usrinfo = _context.UserInfos.SingleOrDefault(p => p.ApplicationUserId == Uid);
            if (usrinfo != null) {
                // 
                var countries = _context.Countries.ToList();
                var vm = new UserInfoFormViewModel
                {
                    userinfo = usrinfo,
                    countries = countries
                };
                return View("Welcome", vm);
            }
            else {
                // the user needs to Create a UserInfo profile
                var ui = new UserInfo {
                    //ApplicationUserId = Uid
                };
                var countries = _context.Countries.ToList();
                var vm = new UserInfoFormViewModel
                {
                    userinfo = ui,
                    countries = countries
                };
                return View("UserInfoForm", vm);
            }
        }
        [HttpPost]
        public IActionResult Save(UserInfoFormViewModel uin)  // uin is short for user information
        {
            if (!ModelState.IsValid)
            {
                var countries = _context.Countries.ToList();
                var vm = new UserInfoFormViewModel
                {
                    userinfo = uin.userinfo,
                    countries = countries
                };
                return View("UserInfoForm", vm);
            }
            if (uin.userinfo.Id == 0)  // ADD NEW USER INFO (UserInfos table in Db)
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var Uid = userManager.GetUserId(User); // Get user id:

                uin.userinfo.ApplicationUserId = Uid;
                _context.UserInfos.Add(uin.userinfo);
            }
            else  // EDIT USER INFO
            {
                var uinfoInDb = _context.UserInfos.Single(p => p.Id == uin.userinfo.Id);
                // Mosh video 44 - manually set or use a DTO for higher level of security,
                // but since we’ve manually set these, DTO probably doesn't increase security.
                uinfoInDb.FirstName = uin.userinfo.FirstName;
                uinfoInDb.LastName = uin.userinfo.LastName;
                uinfoInDb.SIN = uin.userinfo.SIN;
                uinfoInDb.CountryId = uin.userinfo.CountryId;
                // do not edit the Id or the ApplicationUserId because they stay the same
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var Uid = userManager.GetUserId(User); // Get application user id:
            // get the User Info (Profile) from the database's UserInfos table
            var uinfoInDb = _context.UserInfos.Single(i => i.ApplicationUserId == Uid);

            if (uinfoInDb == null)
            {
                return View("Error"); // change this, but leave it for now.
            }
            var ui = new UserInfo
            {
                Id = uinfoInDb.Id,
                FirstName = uinfoInDb.FirstName,
                LastName = uinfoInDb.LastName,
                SIN = uinfoInDb.SIN,
                CountryId = uinfoInDb.CountryId
                // No need to pass ApplicationUserId out to user because in the Save() action
                // we lookup the project anyway by project Id in Profiles table and then we
                // have the ApplicationUserId. It won't change. User cannot edit that. Security.
            };
            var countries = _context.Countries.ToList();
            var vm = new UserInfoFormViewModel
            {
                userinfo = ui,
                countries = countries
            };
            return View("UserInfoForm", vm);
        }
        [HttpGet]
        public ActionResult Create()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var Uid = userManager.GetUserId(User); // Get application user id:
            var ui = new UserInfo
            {
                ApplicationUserId = Uid
            };
            var countries = _context.Countries.ToList();
            var vm = new UserInfoFormViewModel
            {
                userinfo = ui,
                countries = countries
            };
            return View("UserInfoForm", vm);  // show the form 
        }
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            // if Id is not null or zero... 
            if (Id != 0)
            {
                var uInfo = _context.UserInfos.Find(Id);
                _context.UserInfos.Remove(uInfo); // value cannot be null
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
