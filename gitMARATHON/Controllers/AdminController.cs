using gitMARATHON.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks; // Це не потрібно для Task з вашої моделі
using gitMARATHON.Data;
using Microsoft.EntityFrameworkCore;

namespace gitMARATHON.Controllers
{
    [Authorize(Roles = "Admin")] // Тільки для адміністратора
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        // Перегляд всіх лекцій
        public IActionResult ManageLectures()
        {
            var lectures = _context.Lectures.ToList();
            return View(lectures);
        }

        // Додати лекцію
        public IActionResult CreateLecture()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateLecture(Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                _context.Lectures.Add(lecture);
                _context.SaveChanges();
                return RedirectToAction("ManageLectures");
            }
            return View(lecture);
        }

        // Редагування лекції
        public IActionResult EditLecture(int id)
        {
            var lecture = _context.Lectures.Find(id);
            if (lecture == null)
                return NotFound();
            return View(lecture);
        }

        [HttpPost]
        public IActionResult EditLecture(Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                _context.Lectures.Update(lecture);
                _context.SaveChanges();
                return RedirectToAction("ManageLectures");
            }
            return View(lecture);
        }

        // Видалення лекції
        public IActionResult DeleteLecture(int id)
        {
            var lecture = _context.Lectures.Find(id);
            if (lecture != null)
            {
                _context.Lectures.Remove(lecture);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageLectures");
        }

        // Перегляд всіх завдань
        public IActionResult ManageTasks()
        {
            var tasks = _context.Tasks.ToList(); // Явно вказано, що це `gitMARATHON.Models.Task`
            return View(tasks);
        }

        // Додати завдання
        public IActionResult CreateTask()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTask(gitMARATHON.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(task);
                _context.SaveChanges();
                return RedirectToAction("ManageTasks");
            }
            return View(task);
        }

        // Редагування завдання
        public IActionResult EditTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
                return NotFound();
            return View(task);
        }

        [HttpPost]
        public IActionResult EditTask(gitMARATHON.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Update(task);
                _context.SaveChanges();
                return RedirectToAction("ManageTasks");
            }
            return View(task);
        }

        // Видалення завдання
        public IActionResult DeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageTasks");
        }

        // Перегляд студентів
        public async Task<IActionResult> ManageScores()
        {
            var scores = await _context.Scores
                .Include(s => s.User)
                .Where(s => !s.User.IsAdmin)
                .ToListAsync();

            return View(scores);
        }


    }
}
