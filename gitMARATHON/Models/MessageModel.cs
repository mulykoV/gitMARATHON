namespace gitMARATHON.Models
{
    public class MessageModel
    {
        public string Id { get; set; }        // Унікальний ідентифікатор повідомлення
        public string Username { get; set; }  // Ім'я користувача
        public string Text { get; set; }      // Текст повідомлення
        public DateTime Timestamp { get; set; } // Час відправлення повідомлення
    }
}
