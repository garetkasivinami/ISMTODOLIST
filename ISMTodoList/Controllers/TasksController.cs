using ISMTodoList.Models;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ISMTodoList.Controllers
{
    public class TasksController : Controller
    {
        private UserContext db = new UserContext();
        // GET: Tasks
        [Authorize]
        public ActionResult Index(int? page, string searchName, string description)
        {
            List<UserTask> userTasks = new List<UserTask>();
            string userName = User.Identity.Name;
            var usersResult = db.Users.Where((a) => a.Email == userName);
            if (usersResult.Count() == 0)
            {
                return RedirectToAction("Index", "Home", null);
            }
            User currentUser = usersResult.First();
            userTasks = db.UserTasks.Where((a) => a.UserId == currentUser.Id).ToList();
            if (searchName == null && description == null)
            {
                int numberPage = page ?? 0;
                int userTasksCount = userTasks.Count();
                ViewBag.ButtonsFlags = 3;
                if (numberPage <= 0)
                {
                    numberPage = 0;
                    ViewBag.ButtonsFlags = 2;
                    if (userTasksCount >= 10)
                    {
                        userTasks.RemoveRange(numberPage * 10 + 10, userTasksCount - numberPage * 10 - 10);
                    }
                }
                else if (numberPage >= userTasksCount / 10)
                {
                    ViewBag.ButtonsFlags = 1;
                    numberPage = userTasksCount / 10;
                    userTasks.RemoveRange(0, numberPage * 10);
                }
                else
                {
                    if (userTasksCount >= 10)
                    {
                        userTasks.RemoveRange(numberPage * 10 + 10, userTasksCount - numberPage * 10 - 10);
                    }
                    userTasks.RemoveRange(0, numberPage * 10);
                }
                if (userTasksCount < 10)
                {
                    ViewBag.ButtonsFlags = 0;
                }
                ViewBag.Page = numberPage;
                ViewBag.SearchFlags = 0;
            }
            else
            {
                if (searchName != null && searchName.Length > 0)
                {
                    searchName = searchName.ToLower();
                    Regex regex = new Regex($"{searchName}");
                    userTasks = userTasks.Where((a) => (a.Name == null) ? false : regex.IsMatch(a.Name.ToLower())).ToList();
                }
                if (description != null && description.Length > 0)
                {
                    description = description.ToLower();
                    Regex regex = new Regex($"{description}");
                    userTasks = userTasks.Where((a) => (a.Description == null) ? false : regex.IsMatch(a.Description.ToLower())).ToList();
                }
                ViewBag.SearchFlags = 1;
            }
            return View(userTasks);
        }
        [Authorize]
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string targetName, string descKeyWord)
        {
            return RedirectToAction("Index", "Tasks", new { searchName = targetName, description = descKeyWord });
        }
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserTask model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = null;
                string userName = User.Identity.Name;
                user = db.Users.FirstOrDefault(u => u.Email == userName);
                if (user == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                model.UserId = user.Id;
                db.UserTasks.Add(model);
                db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(model);
        }
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home", null);
            }
            UserTask userTask = null;
            User user = null;
            userTask = db.UserTasks.Find(id);
            if (userTask == null)
            {
                return RedirectToAction("Index", "Home", null);
            }
            string userName = User.Identity.Name;
            user = db.Users.FirstOrDefault(u => u.Email == userName);
            if (user == null || user.Id != userTask.UserId)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(userTask);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserTask userTask)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                string userName = User.Identity.Name;
                user = db.Users.FirstOrDefault(u => u.Email == userName);
                if (user == null || user.Id != userTask.UserId)
                {
                    return RedirectToAction("Index", "Home");
                }
                db.Entry(userTask).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userTask);
        }
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home", null);
            }
            UserTask userTask = null;
            userTask = db.UserTasks.Find(id);
            if (userTask == null)
            {
                return RedirectToAction("Index", "Home", null);
            }
            User user = null;
            string userName = User.Identity.Name;
            user = db.Users.FirstOrDefault(u => u.Email == userName);
            if (user == null || user.Id != userTask.UserId)
            {
                return RedirectToAction("Index", "Home");
            }
            db.UserTasks.Remove(userTask);
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