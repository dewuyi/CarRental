using Coravel;
using NotificationService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext,services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddHostedService<Worker>(); 
        services.Configure<Settings>(configuration.GetSection("SendGridApiKey"));
        services.AddScheduler();
        services.AddSingleton<ExpiringRentalNotificationService>();
        services.AddSingleton<IEmailService, EmailService>();
    })
    .Build();

host.Services.UseScheduler(scheduler => {
    // We'll fill this in later ;)
    scheduler.Schedule<NotificationService.ExpiringRentalNotificationService>()
        .EveryFiveMinutes();
});

await host.RunAsync();