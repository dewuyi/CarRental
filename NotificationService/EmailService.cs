using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NotificationService;

public class EmailService:IEmailService
{
    private SendGridClient _client;
    private readonly Settings _settings;
    

    public EmailService(IOptions<Settings> settings)
    {
        _settings = settings.Value;
        _client = new SendGridClient(_settings.SendGridApiKey);
    }

    public async Task<Response> SendEmail(SendGridMessage message)
    {
       return await _client.SendEmailAsync(message);
    }
}