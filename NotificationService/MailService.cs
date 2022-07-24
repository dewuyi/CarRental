using CarRental.Model;
using CarRental.Repository;
using Coravel.Invocable;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NotificationService;

public class MailService:IInvocable
{
    private readonly IRentalRepository _rentalRepository;
    private readonly Settings _settings;
    private Queue<NotificationMessage> _messagesToSend;
    private SendGridClient _client;
    private IEnumerable<Rental> _rentalCache;
    private DateTime _rentalCacheLastUpdate = DateTime.MinValue;
    public MailService(IRentalRepository rentalRepository, 
        IOptions<Settings> settings,
        Queue<NotificationMessage> messagesToSend, 
        IEnumerable<Rental> rentalCache)
    {
        _rentalRepository = rentalRepository;
        _messagesToSend = messagesToSend;
        _rentalCache = rentalCache;
        _settings = settings.Value;
        _client = new SendGridClient(_settings.SendGridApiKey);
    }

    public async Task Invoke()
    {
        // refresh cache if it has expired
        if (DateTime.Now.AddHours(-_settings.CacheExpiryHours) >= _rentalCacheLastUpdate)
        {
            _rentalCache = await _rentalRepository.GetExpiringRental();
        }
        
        var message = new SendGridMessage()
        {
            From = new EmailAddress("Banjioyawoye@gmail.com", "Adewuyi"),
            Subject = "Your rental contract is expiring in less than 6 hours",
            PlainTextContent = "Please return the vehicle by the due time today"
        };
        
        foreach (var notificationMessage in _rentalCache.Select(rental => new NotificationMessage
                 {
                     Message = message,
                     CustomerEmail = rental.CustomerEmail,
                     CustomerName = rental.CustomerName
                 }))
        {
            _messagesToSend.Enqueue(notificationMessage);
        }

        await SendMessages();
    }

    private async Task SendMessages()
    {
        foreach (var message in _messagesToSend)
        {
            message.Message.AddTo(new EmailAddress(message.CustomerEmail, message.CustomerName));
            var response = await _client.SendEmailAsync(message.Message);
            if (response.IsSuccessStatusCode)
            {
                _messagesToSend.Dequeue();
            }
        }
    }
    
}