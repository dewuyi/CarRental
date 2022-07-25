using SendGrid;
using SendGrid.Helpers.Mail;

namespace NotificationService;

public interface IEmailService
{
    Task<Response> SendEmail(SendGridMessage message);
}