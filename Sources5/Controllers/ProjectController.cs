using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sources5.Data;
using Sources5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Sources5.Controllers
{
    [Authorize(Roles = "Admin,Users")]
    public class ProjectController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context, UserManager<ApplicationUser> usrMgr)
        {
            _context = context;
            userManager = usrMgr;
        }
        public IActionResult Create()
        {
            
            var s = Environment.CurrentDirectory;  // JUST FOR FUN...

            // The user has clicked the 'Create New Project' button in the Welcome view
            // This is just an anchor link <a> in Welcome.cshtml
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var Uid = userManager.GetUserId(User); // Get user id (string)
            // Get the row from our UserInfos table that is the current user's
            var userinfo = _context.UserInfos.SingleOrDefault(i => i.ApplicationUserId == Uid);
            var project = new Project
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                UserInfoId = userinfo.Id
            };
            return View("ProjectForm2", project);  // show the form
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Project project)
        {
            if (!ModelState.IsValid)
            {
                return View("ProjectForm2", project);
            }

            if (project.Id == 0)       // ADD NEW PROJECT (Projects table in Db)
            {
                // create a new Project object and use object initializer syntax to 
                // initialize all of the properties except for:
                //    Id - this is an Identity in Db - do not initialize it - the Db will
                //    UserInfo - this is a navigation property
                var p = new Project()
                {
                    // Id = proj.Id,      AN IDENTITY THAT THE DB WILL INITIALIZE
                    Name = project.Name,
                    Description = project.Description,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    UserInfoId = project.UserInfoId  // foreign key
                };
                _context.Projects.Add(p);
            }
            else                    // EDIT PROJECT (Projects table in Db)
            {
                var projInDb = _context.Projects.Single(p => p.Id == project.Id);
                // Mosh video 44 - manually set these (or use a DTO for higher level of security)
                projInDb.Name = project.Name;
                projInDb.Description = project.Description;
                projInDb.StartDate = project.StartDate;
                projInDb.EndDate = project.EndDate;
                // do not edit the Id or the ApplicationUserId because they stay the same
            }
            _context.SaveChanges();
            string msgToast = $"{project.Name} has been saved"; // A. Freeman p 310 of Pro ASP.NET
            return RedirectToAction("Index", "Project", new { msgToastr = msgToast });
        }
        [HttpGet]
        public IActionResult Index(string msgToastr)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var Uid = userManager.GetUserId(User); // Get user id (string)
            // get the user from our UserInfos table.
            UserInfo usrinfo = _context.UserInfos.SingleOrDefault(p => p.ApplicationUserId == Uid);
            // if the user has already created their Profile then go to list of their Projects
            // (Welcome) even if they don't have any projects yet. 
            if (usrinfo != null)
            {
                
                // get the list of this user's projects, if any. 
                IEnumerable<Project> projects = _context.Projects.Where(p => p.UserInfoId == usrinfo.Id).ToList();
                var vm = new WelcomeProjectViewModel
                {
                    projects = projects,
                    nameproject = msgToastr // this is for the toastr by CodeSeven
                };
                return View("Welcome", vm); // pass project list to the Welcome view
            }
            else {
                // user must create a profile before they can create projects.
                return RedirectToAction("Create", "UserInfo");  // (action, controller)
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id)
        {
            // get the project row from the database table Projects that has Id of Id 
            var pjInDb = _context.Projects.Single(p => p.Id == Id);
            if (pjInDb != null)  // project found in database
            {
                var pj = new Project
                {   // object initialization syntax
                    Id = pjInDb.Id,  // changed this to Id = pjInDb.Id
                    Name = pjInDb.Name,
                    Description = pjInDb.Description,
                    StartDate = pjInDb.StartDate,
                    EndDate = pjInDb.EndDate,
                    UserInfoId = pjInDb.UserInfoId
                };
                return View("ProjectForm2", pj);
            }
            return View("Error"); // using Single() would throw an error making this code redundant
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int Id)
        {
            // if projectId is not null or zero... 
            if (Id != 0)
            {
                var project = _context.Projects.Find(Id);
                var projectname = project.Name;
                _context.Projects.Remove(project); // value cannot be null
                _context.SaveChanges();
                return RedirectToAction("Index", "Project", new { msgToastr = $"{projectname} has been deleted" });
            }
            else
            {
                return View("Error");
            }
            
        }
    }
}
