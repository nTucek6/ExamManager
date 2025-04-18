using Services.EmailSender;

namespace ExamManager.API.Services
{
    public class SendEmailTimer : IHostedService, IDisposable
    {
        private readonly ILogger<SendEmailTimer> _logger;
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;

        public SendEmailTimer(ILogger<SendEmailTimer> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("TimerService is starting.");

            var now = DateTime.Now;
            var desiredTimeUtc1 = new DateTime(now.Year, now.Month, now.Day, 08, 00, 00, DateTimeKind.Local);
           

            if (desiredTimeUtc1 <= now)
            {
                desiredTimeUtc1 = desiredTimeUtc1.AddDays(1);
            }

            var initialDelay1 = desiredTimeUtc1 - now;
          
            _timer = new Timer(async state => await DoWorkAsync(state), null, initialDelay1, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }
        private async Task DoWorkAsync(object state)
        {
            _logger.LogInformation("TimerService is working at specific time.");
            // Perform your asynchronous background task here

            using var scope = _serviceProvider.CreateScope();

            var mySingletonService = scope.ServiceProvider.GetRequiredService<IEmailSenderService>();
            await mySingletonService.SendEmailToStudents();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("TimerService is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}
