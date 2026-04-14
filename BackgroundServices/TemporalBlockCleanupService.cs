using BlockedCountries.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.BackgroundServices
{
    public class TemporalBlockCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TemporalBlockCleanupService> _logger;

        public TemporalBlockCleanupService(IServiceProvider serviceProvider, ILogger<TemporalBlockCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Temporal Block Cleanup Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var tempRepo = scope.ServiceProvider.GetRequiredService<ITemporaryBlockRepository>();

                    _logger.LogInformation("Checking for expired temporal blocks...");
                    tempRepo.RemoveExpiredBlocks();
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
