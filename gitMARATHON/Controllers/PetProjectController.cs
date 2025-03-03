using gitMARATHON.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace gitMARATHON.Controllers
{
    [Authorize] // Доступ тільки для зареєстрованих користувачів
    public class PetProjectController : Controller
    {
        private static List<PetProject> _projects = new List<PetProject>();

        public IActionResult Index()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userProjects = _projects.Where(p => p.UserId == currentUserId || User.IsInRole("Admin")).ToList();
            return View(userProjects);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PetProject project)
        {
            if (ModelState.IsValid)
            {
                project.Id = _projects.Count + 1;
                project.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                project.IsUnlocked = CheckIfUserCompletedAllTasks(project.UserId);
                _projects.Add(project);
                return RedirectToAction("Index");
            }
            return View(project);
        }

        public IActionResult Edit(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id && p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost]
        public IActionResult Edit(PetProject project)
        {
            var existingProject = _projects.FirstOrDefault(p => p.Id == project.Id && p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (existingProject != null)
            {
                existingProject.GitHubLink = project.GitHubLink;
                return RedirectToAction("Index");
            }
            return View(project);
        }

        public IActionResult Delete(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id && p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (project != null)
            {
                _projects.Remove(project);
            }
            return RedirectToAction("Index");
        }

        private bool CheckIfUserCompletedAllTasks(string userId)
        {
            // Логіка перевірки виконання всіх завдань
            return true; // Поки що вважаємо, що всі виконані
        }
    }
}
