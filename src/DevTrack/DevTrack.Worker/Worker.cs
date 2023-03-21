using Autofac;
using DevTrack.Infrastructure.Services.Email;
using DevTrack.Worker.WorkerServices;

namespace DevTrack.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<Worker> _logger;

        public Worker(
            ILifetimeScope scope,
            ILogger<Worker> logger)
        {
            _scope = scope;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                EmailSender emailSender = _scope.Resolve<EmailSender>();
                emailSender.ResolveDependency(_scope, _logger);
                await emailSender.sendEmails();

                await Task.Delay(5 * 1000, stoppingToken);
            }
        }
    }
}
