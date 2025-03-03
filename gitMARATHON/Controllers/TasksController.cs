using gitMARATHON.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class TasksController : Controller
{
    private readonly ApplicationDbContext _context;

    public TasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Для перегляду завдань
    public IActionResult Index()
    {
        var tasks = _context.Tasks.ToList();
        return View(tasks);
    }

    // Для редагування завдань (лише для адміна)
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    // Для перегляду конкретного завдання
    public IActionResult View(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }
}
