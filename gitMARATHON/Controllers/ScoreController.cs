using gitMARATHON.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using gitMARATHON.Data;

namespace gitMARATHON.Controllers
{
    [Authorize]
    public class ScoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userScores = User.IsInRole("Admin")
                ? await _context.Scores.Include(s => s.Lecture).ToListAsync()
                : await _context.Scores.Where(s => s.UserId == currentUserId).Include(s => s.Lecture).ToListAsync();

            return View(userScores);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageScores(string userId)
        {
            var userScores = await _context.Scores.Where(s => s.UserId == userId)
                                                  .Include(s => s.Lecture)
                                                  .ToListAsync();
            ViewBag.UserId = userId;
            return View(userScores);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Assign(int lectureId, string userId)
        {
            var score = new Score { LectureId = lectureId, UserId = userId };
            return View(score);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Assign(Score score)
        {
            if (ModelState.IsValid)
            {
                var existingScore = await _context.Scores.FirstOrDefaultAsync(s => s.UserId == score.UserId && s.LectureId == score.LectureId);
                if (existingScore == null)
                {
                    _context.Scores.Add(score);
                }
                else
                {
                    existingScore.Points = score.Points;
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageScores", new { userId = score.UserId });
            }
            return View(score);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var score = await _context.Scores.FindAsync(id);
            if (score == null) return NotFound();
            return View(score);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Score score)
        {
            var existingScore = await _context.Scores.FindAsync(score.Id);
            if (existingScore != null)
            {
                existingScore.Points = score.Points;
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageScores", new { userId = score.UserId });
            }
            return View(score);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var score = await _context.Scores.FindAsync(id);
            if (score != null)
            {
                _context.Scores.Remove(score);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
