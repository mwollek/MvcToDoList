using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MvcToDoList.Models;
using MvcToDoList.Models.Own;
using MvcToDoList.Models.ViewModels;

namespace MvcToDoList.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();

        // GET: Tasks
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var tasks = dbContext.Tasks.Include(t => t.ApplicationUser).Where(t => t.ApplicationUserId == userId).ToList();
            List<TaskViewModel> models = new List<TaskViewModel>();

            tasks.ForEach(x => models.Add(new TaskViewModel()
            {
                TaskId = x.TaskId,
                Title = x.Title.Length > 28 ? x.Title.Substring(0, 25) + "..." : x.Title,
                Status = Task.ProgressStatesDict[x.ProgressState],
                FinishDate = x.PlannedFinishDate.ToShortDateString(),
                DaysLeftMessage = (x.PlannedFinishDate.DayOfYear - DateTime.Now.DayOfYear) >= 0 
                                    ? (x.PlannedFinishDate.DayOfYear - DateTime.Now.DayOfYear).ToString() : "missed",
                ModifyButtonLinkText = x.ProgressState == (int)Task.ProgressStatesEnum.Done ? "Undo" : "Done",
                ButtonStyle = x.ProgressState == (int)Task.ProgressStatesEnum.Done ? "warning" : "success"
            }));

            return View(models);
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = dbContext.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Task task)
        {
            if (ModelState.IsValid)
            {
                task.ApplicationUserId = User.Identity.GetUserId();
                dbContext.Tasks.Add(task);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(task);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = dbContext.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Task task, bool done)
        {
            if (ModelState.IsValid)
            {
                task.ProgressState = done == true ? (int)Task.ProgressStatesEnum.Done : (int)Task.ProgressStatesEnum.InProgress;
                dbContext.Entry(task).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = dbContext.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = dbContext.Tasks.Find(id);
            dbContext.Tasks.Remove(task);
            dbContext.SaveChanges();
            TempData["message"] = "Task succesfully deleted";
            return RedirectToAction("Index");
        }

        public ActionResult ModifyStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = dbContext.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            switch (task.ProgressState)
            {
                case (int)Task.ProgressStatesEnum.Done:
                    task.ProgressState = (int)Task.ProgressStatesEnum.InProgress;                   
                    break;
                case (int)Task.ProgressStatesEnum.InProgress:
                    task.ProgressState = (int)Task.ProgressStatesEnum.Done;
                    break;
                default:
                    break;
            }
            TempData["message"] = $"Set task status to: {Task.ProgressStatesDict[task.ProgressState]}";
            dbContext.Entry(task).State = EntityState.Modified;
            dbContext.SaveChanges();
            
            return RedirectToAction("Index");
        }
   
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
