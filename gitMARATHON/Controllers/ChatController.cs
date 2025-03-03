using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using gitMARATHON.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using gitMARATHON.Data;

[Route("chat")]
public class ChatController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _dbContext;

    public ChatController(UserManager<User> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    // Відправка повідомлення
    [HttpPost("sendMessage")]
    public async Task<IActionResult> SendMessage([FromBody] MessageModel message)
    {
        if (message == null || string.IsNullOrWhiteSpace(message.Text))
        {
            return BadRequest("Повідомлення не може бути порожнім.");
        }

        var user = await _userManager.GetUserAsync(User);
        message.Username = user?.UserName ?? "Анонім";
        message.Id = Guid.NewGuid().ToString(); // Генерація унікального Guid
        message.Timestamp = DateTime.UtcNow;      // Запис часу відправлення

        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync();

        // Повертаємо лише необхідні дані
        return Ok(new
        {
            messageId = message.Id,
            text = message.Text,
            username = message.Username
        });
    }

    // Отримання всіх повідомлень
    [HttpGet("getMessages")]
    public IActionResult GetMessages()
    {
        var messages = _dbContext.Messages.ToList();
        return Ok(messages);
    }

    // Видалення повідомлення (тільки для адміністраторів)
    [HttpPost("deleteMessage")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMessage([FromBody] DeleteMessageRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.MessageId))
        {
            return BadRequest("Неправильні дані.");
        }

        var message = await _dbContext.Messages.FindAsync(request.MessageId);
        if (message != null)
        {
            _dbContext.Messages.Remove(message);
            await _dbContext.SaveChangesAsync();
            return Ok(new { messageId = message.Id, status = "deleted" });
        }

        return NotFound("Повідомлення не знайдено.");
    }

    // Модель запиту на видалення повідомлення
    public class DeleteMessageRequest
    {
        public string MessageId { get; set; }
    }
}
