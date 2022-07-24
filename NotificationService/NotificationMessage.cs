using CarRental.Model;
using SendGrid.Helpers.Mail;

namespace NotificationService;

public class NotificationMessage
{
    public SendGridMessage Message { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
}