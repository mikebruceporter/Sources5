using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Sources5.Data;
using Sources5.Models;
using Microsoft.AspNetCore.Authorization;
namespace Sources5.Controllers
{
    [Authorize(Roles = "Admin,Users")]
    public class ProjectTaskController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private ApplicationDbContext _context;
        public ProjectTaskController(ApplicationDbContext context, UserManager<ApplicationUser> usrMgr)
        {
            _context = context;
            userManager = usrMgr;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TaskListForProject(int pId)
        {
            // The user clicked one of the Tasks buttons in the list of projects.
            // The Id of the project is passed in as pId. How?
            // It is a form with method post.
            var project = _context.Projects.SingleOrDefault(p => p.Id == pId);
            if (project != null) // user has some projects.
            {
                var tasks = _context.ProjectTasks.Where(t => t.ProjectId == pId).ToList();
                var vm = new WelcomeTasksViewModel()
                {
                    project = project,
                    tasks = tasks,
                    priorities = _context.Priorities.ToList(),
                    nametask = ""
                };
                return View("Welcome", vm);
            }
            else { return View("Error"); }
        }
        [HttpGet]
        public IActionResult Index(string msgToastr)
        {
            // The user has clicked the Cancel, Delete or Save button.
            // (a) The user has clicked the Cancel button in the Task form. 
            // (b) The user has clicked the Delete button to delete a task.
            // (c) The user has either created a new task or edited an existing task
            //     and clicked the Save button. We have already saved the task. 
            // We need to display the Welcome page that shows all of the tasks
            // for THIS project. TempData contains the project Id.
            // The Save, Delete and Cancel actions REDIRECT to Index().
            var projectId = (int)TempData["projectId"];

            var project = _context.Projects.SingleOrDefault(p => p.Id == projectId);
            if (project != null) // user has some projects.
            {
                //
                var tasks = _context.ProjectTasks.Where(t => t.ProjectId == projectId).ToList();
                // _context.ProjectTasks.Include(t => t.Priority)
                //        .Where(t => t.ProjectId == projectId).ToList();
                var vm = new WelcomeTasksViewModel()
                {
                    project = project,
                    tasks = tasks,
                    priorities = _context.Priorities.ToList(),
                    nametask = msgToastr
                };
                return View("Welcome", vm);
            }
            else
            {
                return View("Error"); // User has no projects. This should never happen.
            } 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Id)
        {
            // The Id passed in is the Id number of the TASK in the ProjectTasks table.
            // the user has clicked on a specific Task with this Id
            // get the task (single row) in the database table ProjectTasks for this project
            //var taskInDb = _context.ProjectTasks.Include(t => t.Priority).SingleOrDefault(t => t.Id == Id);
            var taskInDb = _context.ProjectTasks.SingleOrDefault(t => t.Id == Id);
            // notice above we are including priorities.
            // var taskInDbFind = _context.ProjectTasks.Include(t => t.Priority).Find(Id);  
            // I don't think we can use Find() as Find gives error.
            var projectInDb = _context.Projects.Find(taskInDb.ProjectId);
            if (taskInDb != null)
            {
                var task = new ProjectTask
                {
                    Id = taskInDb.Id,
                    Name = taskInDb.Name,
                    Description = taskInDb.Description,
                    Methodology = taskInDb.Methodology,
                    PriorityId = taskInDb.PriorityId,
                    ProjectId = taskInDb.ProjectId
                };
                var vm = new TaskFormViewModel()
                {
                    project = projectInDb,
                    task = task,
                    priorities = _context.Priorities.ToList()
                };
                return View("ProjectTaskForm2", vm);
            }
            else
            {
                return View("Error"); // this is really an error!
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int pId)
        {
            // The user has clicked the Create New Task button.
            // We need to display the form.
            // However, we need to know the project.
            // Create a new task for the project with id of pId.
            var projectInDb = _context.Projects.Find(pId);
            var task = new ProjectTask
            {
                ProjectId = pId
            };
            var vm = new TaskFormViewModel()
            {
                project = projectInDb,
                task = task,
                priorities = _context.Priorities.ToList() 
            };
            //return View("ProjectTaskForm", t);
            return View("ProjectTaskForm2", vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(TaskFormViewModel tk)  //  
        {
            if (!ModelState.IsValid)
            {   // if not valid send data back and display again
                var vm = new TaskFormViewModel
                {   // need to re-build and send back vm, do not send back tk!
                    task = tk.task,
                    priorities = _context.Priorities.ToList(),
                    project = _context.Projects.Single(p => p.Id == tk.task.ProjectId)
                };
                return View("ProjectTaskForm2", vm);
            }
            // if (pri == 0) {return View("ProjectTaskForm", tk); } // This code never reached probably 
            //    because it his above !ModelState.IsValid
            if (tk.task.Id == 0)  // ADD NEW TASK (ProjectTasks table)
            {
                var task = new ProjectTask()
                {
                    Name = tk.task.Name,
                    Description = tk.task.Description,
                    Methodology = tk.task.Methodology,
                    PriorityId = tk.task.PriorityId,
                    ProjectId = tk.task.ProjectId // is in the form, but hidden
                    //Id = projtask.Id    since this is NEW, the Db Identity will take care of this
                };
                _context.ProjectTasks.Add(task);
            }
            else  // EDIT TASK
            {
                var taskInDb = _context.ProjectTasks.Single(t => t.Id == tk.task.Id);
                // Mosh video 44 - can manually set or use a DTO for higher level of security,
                // but since we’ve manually set these, DTO probably doesn't increase security.
                taskInDb.Name = tk.task.Name;
                taskInDb.Description = tk.task.Description;
                taskInDb.Methodology = tk.task.Methodology;
                taskInDb.PriorityId = tk.task.PriorityId;   
                //taskInDb.ProjectId = tk.task.ProjectId    we don't need this we already have it in Db
                //taskInDb.Id = tk.task.Id; we don't need to re-set this; cannot edit this
            }
            _context.SaveChanges();

            TempData["message"] = $"{tk.task.Name} has been saved"; //page 312 in book called Pro ASP.NET Core MVC 2
            TempData["projectId"] = tk.task.ProjectId;  // is in the form, but hidden
            string msgToast = $"{tk.task.Name} has been saved";
            return RedirectToAction("Index", "ProjectTask", new { msgToastr = msgToast });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int Id)
        {
            // Delete a task in the ProjectsTasks table that has an Id of the Id passed in.
            // if projectId is not null or zero... 
            if (Id != 0)
            {
                var task = _context.ProjectTasks.Find(Id);
                TempData["projectId"] = task.ProjectId;  // PROJECT Id not task Id
                _context.ProjectTasks.Remove(task); // value cannot be null
                _context.SaveChanges();
                string msgToast = $"{task.Name} has been deleted.";
                return RedirectToAction("Index", "ProjectTask", new { msgToastr = msgToast });
            }
            else { return View("Error"); }
        }
        [HttpPost]
        public IActionResult Cancel(int Id)
        {
            TempData["projectId"] = Id;
            return RedirectToAction("Index");
        }
    }
}
