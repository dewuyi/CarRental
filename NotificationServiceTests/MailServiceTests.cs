using System.Net;
using AutoFixture;
using AutoFixture.AutoMoq;
using CarRental.Model;
using CarRental.Repository;
using Moq;
using NotificationService;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NotificationServiceTests;

public class MailServiceTests
{
    private Fixture _fixture;
    private Mock<IRentalRepository> _mockRentalRepository;
    private Mock<IEmailService> _mockEmailService;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoMoqCustomization());
        _mockRentalRepository = _fixture.Freeze<Mock<IRentalRepository>>();
        _mockEmailService = _fixture.Freeze<Mock<IEmailService>>();
    }
    
    [Test]
    public async Task NotifyAlerts_ForExpiring_Rentals()
    {
        var sut = _fixture.Create<ExpiringRentalNotificationService>();
        _mockRentalRepository.Setup(repo => repo.GetExpiringRental()).ReturnsAsync(GetExpiringRentals);
        _mockEmailService.Setup(s => s.SendEmail(It.IsAny<SendGridMessage>())).ReturnsAsync(new Response(
            HttpStatusCode.OK,
            null, null));
         await sut.Invoke();
         _mockEmailService.Verify(s=>s.SendEmail(It.IsAny<SendGridMessage>()), Times.Exactly(2));
    }

    private List<Rental> GetExpiringRentals()
    {
        return new List<Rental>
        {
            new()
            {
                RentalStart = new DateTime(2022, 06, 01),
                RentalEnd = new DateTime(2022, 06, 06),
                CustomerEmail = "Jon@gmail.com",
                CustomerName = "Jon"
            },
            new()
            {
                RentalStart = new DateTime(2022, 06, 05),
                RentalEnd = new DateTime(2022, 06, 08),
                CustomerEmail = "Jay@gmail.com",
                CustomerName = "Jay"
            }
        };
    }
}