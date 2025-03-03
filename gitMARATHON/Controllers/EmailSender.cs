using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

public class EmailSender
{
    private readonly string _smtpServer = "smtp.gmail.com"; // Замінити на свій
    private readonly int _port = 587;
    private readonly string _fromEmail = "dnzz.fit@gmail.com";
    private readonly string _password = "gedi cigu sorb kzsq";

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Git Marathon", _fromEmail));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;

        emailMessage.Body = new TextPart("plain")
        {
            Text = message
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_smtpServer, _port, false);
            await client.AuthenticateAsync(_fromEmail, _password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
