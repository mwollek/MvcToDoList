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
using static MvcToDoList.Models.Own.Task;

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
            var tasks = dbContext.Tasks
                .Include(t => t.ApplicationUser)
                .Where(t => t.ApplicationUserId == userId)
                .Where(t => t.ProgressState != ProgressStates.Confirmed)
                .ToList();
            List<TaskViewModel> models = new List<TaskViewModel>();

            tasks.ForEach(x =>
            {
                var daysToDeadline = x.PlannedFinishDate.DayOfYear - DateTime.Now.DayOfYear;
                string thumbnailStyle = "";

                if (daysToDeadline >= 0 && x.ProgressState == ProgressStates.Done)
                    thumbnailStyle = "background-color:#F3FDEC";
                if (daysToDeadline < 0 && x.ProgressState == ProgressStates.Done)
                    thumbnailStyle = "background-color:#FCD9DB";

                models.Add(new TaskViewModel()
                {
                    TaskId = x.TaskId,
                    Title = x.Title.Length > 28 ? x.Title.Substring(0, 25) + "..." : x.Title,
                    Status = ProgressStatesTextDict[x.ProgressState],
                    PlannedFinishDate = x.PlannedFinishDate.ToShortDateString(),

                    DaysLeftMessage = daysToDeadline >= 0
                                    ? daysToDeadline.ToString() : "passed",

                    ModifyButtonLinkText = x.ProgressState == ProgressStates.Done ? "Undo" : "Done",
                    ButtonStyle = x.ProgressState == ProgressStates.Done ? "warning" : "success",
                    ThumbnailBackgroundStyle = thumbnailStyle
                });
            });
            

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
                task.ProgressState = done == true ? ProgressStates.Done : ProgressStates.InProgress;
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

        public ActionResult ModifyStatus(int? id, bool? confirmed)
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

            if (confirmed == true)
            {
                task.ProgressState = ProgressStates.Confirmed;
                task.ActualFinishedDate = DateTime.Now.Date;
                task.Missed = task.PlannedFinishDate < task.ActualFinishedDate ? true : false;
            }
            else
            {
                switch (task.ProgressState)
                {
                    case ProgressStates.Done:
                        task.ProgressState = ProgressStates.InProgress;
                        break;
                    case ProgressStates.InProgress:
                        task.ProgressState = ProgressStates.Done;
                        break;
                    default:
                        break;
                }
                TempData["message"] = $"Set task status to: {ProgressStatesTextDict[task.ProgressState]}";
            }

            dbContext.Entry(task).State = EntityState.Modified;
            dbContext.SaveChanges();
            
            return RedirectToAction("Index");
        }

        public ViewResult ConfirmedTasks()
        {
            string userId = User.Identity.GetUserId();
            var confirmedTasks = dbContext.Tasks
                .Include(t => t.ApplicationUser)
                .Where(t => t.ApplicationUserId == userId)
                .Where(t => t.ProgressState == ProgressStates.Confirmed)
                .OrderBy(t => t.Missed)
                .ToList();
            List<ConfirmedTaskViewModel> models = new List<ConfirmedTaskViewModel>();

            foreach (Task task in confirmedTasks)
            {
                models.Add(new ConfirmedTaskViewModel()
                {
                    TaskId = task.TaskId,
                    Title = task.Title.Length > 28 ? task.Title.Substring(0, 25) + "..." : task.Title,
                    PlannedFinishDate = task.PlannedFinishDate.ToShortDateString(),
                    ActualFinishDate = task.ActualFinishedDate.GetValueOrDefault().ToShortDateString(),
                    TotalDays =  task.ActualFinishedDate.GetValueOrDefault().DayOfYear - task.CreationDate.DayOfYear,
                    OnTimeStatus = (bool)task.Missed ? "missed" : "on time",
                    OnTimeStatusColour = (bool)task.Missed ? "red" : "green"
                });
            }
            return View(models);
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
