using CarRental.Model;
using CarRental.Repository;
using Coravel.Invocable;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;

namespace NotificationService;

public class ExpiringRentalNotificationService:IInvocable
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IEmailService _emailService;
    private readonly Settings _settings;
    private Queue<NotificationMessage> _messagesQueue = new();
    private IEnumerable<Rental> _rentalCache;
    private DateTime _rentalCacheLastUpdate = DateTime.MinValue;
    public ExpiringRentalNotificationService(IRentalRepository rentalRepository, 
        IOptions<Settings> settings,
        IEnumerable<Rental> rentalCache, IEmailService emailService)
    {
        _rentalRepository = rentalRepository;
        _rentalCache = rentalCache;
        _emailService = emailService;
        _settings = settings.Value;
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
            _messagesQueue.Enqueue(notificationMessage);
        }

        await SendMessages();
    }

    private async Task SendMessages()
    {
        Queue<NotificationMessage> messageToSend = new Queue<NotificationMessage>(_messagesQueue);
        foreach (var message in messageToSend)
        {
            message.Message.AddTo(new EmailAddress(message.CustomerEmail, message.CustomerName));
            var response = await _emailService.SendEmail(message.Message);
            if (response.IsSuccessStatusCode)
            {
                _messagesQueue.Dequeue();
            }
        }
    }
    
}