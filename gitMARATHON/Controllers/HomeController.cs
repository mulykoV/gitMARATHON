using gitMARATHON.Models;
using gitMARATHON.Data; // Додаємо простір імен з ApplicationDbContext
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace gitMARATHON.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Додаємо _context

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // Ініціалізуємо _context
        }

        public IActionResult Index()
        {
            var speakers = new List<Speaker>
            {
                new Speaker { Id = 1, Name = "John Doe", Description = "Senior GitHub Expert", PhotoUrl = "/images/speaker1.jpg" },
                new Speaker { Id = 2, Name = "Jane Smith", Description = "DevOps Engineer", PhotoUrl = "/images/speaker2.jpg" }
            };

            var lectures = new List<Lecture>
            {
                new Lecture { Id = 1, Title = "Git Basics", Description = "Вивчаємо основи Git", VideoPath = "/videos/git_basics.mp4", PracticalTask = "Створити власний репозиторій та зробити коміт" },
                new Lecture { Id = 2, Title = "Branching Strategies", Description = "Як працювати з гілками", VideoPath = "/videos/branching.mp4", PracticalTask = "Створити нову гілку та зробити merge" },
                new Lecture { Id = 3, Title = "GitHub Actions", Description = "CI/CD з GitHub", VideoPath = "/videos/github_actions.mp4", PracticalTask = "Налаштувати GitHub Actions для автотестів" },
                new Lecture { Id = 4, Title = "Pull Requests & Code Review", Description = "Як правильно робити PR та рев'ю коду", VideoPath = "/videos/pull_requests.mp4", PracticalTask = "Створити pull request та додати рев'юера" },
                new Lecture { Id = 5, Title = "GitHub Security", Description = "Як захистити свій код", VideoPath = "/videos/github_security.mp4", PracticalTask = "Налаштувати секрети в GitHub Actions" },
                new Lecture { Id = 6, Title = "Open Source Contribution", Description = "Як брати участь у Open Source", VideoPath = "/videos/open_source.mp4", PracticalTask = "Знайти open-source проєкт і зробити свій перший вклад" }
            };

            var viewModel = new HomeViewModel
            {
                Speakers = speakers,
                Lectures = lectures
            };

            return View(viewModel);
        }

        public IActionResult VideoLibrary()
        {
            var lectures = _context.Lectures.ToList(); // Використовуємо _context для отримання лекцій
            return View(lectures);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Tasks()
        {
            // Отримуємо список завдань з бази даних
            var tasks = _context.Tasks.ToList();  // .ToList() щоб отримати всі записи із таблиці Task
            return View(tasks);  // Передаємо список в View
        }


        public IActionResult Contact()
        {
            var model = new ChatViewModel
            {
                Messages = new List<MessageModel> // Використовуємо MessageModel
            {
                new MessageModel { Id = "1", Username = "admin", Text = "Привіт!", Timestamp = DateTime.Now },
                new MessageModel { Id = "2", Username = "user", Text = "Як справи?", Timestamp = DateTime.Now }
            }
            };

            return View(model);
        }

        public IActionResult Speakers()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
