using gitMARATHON.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace gitMARATHON.Controllers
{
    public class LectureController : Controller
    {
        private static List<Lecture> lectures = new List<Lecture>(); // Тимчасова база лекцій (заміни на БД)
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LectureController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(lectures);
        }

        public IActionResult Details(int id)
        {
            var lecture = lectures.FirstOrDefault(l => l.Id == id);
            if (lecture == null) return NotFound();
            return View(lecture);
        }

        // Форма для додавання лекції (адмін)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Lecture model)
        {
            if (ModelState.IsValid)
            {
                model.Id = lectures.Count + 1;
                lectures.Add(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // Завантаження відео (адмін)
        [HttpPost]
        public IActionResult UploadVideo(int id, IFormFile videoFile)
        {
            var lecture = lectures.FirstOrDefault(l => l.Id == id);
            if (lecture == null || videoFile == null) return BadRequest();

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "videos");
            string uniqueFileName = $"{Guid.NewGuid()}_{videoFile.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                videoFile.CopyTo(fileStream);
            }

            lecture.VideoPath = $"/videos/{uniqueFileName}"; // Зберігаємо шлях до відео
            return RedirectToAction("Details", new { id });
        }
    }
}
