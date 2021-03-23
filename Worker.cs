using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;
        private readonly string website = "https://google.kg/";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            _logger.LogInformation("The service has been stopped...");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await client.GetAsync(website);
                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("The website {website} is up. Status code {StatusCode}", website, result.StatusCode);
                }
                else
                {
                    _logger.LogError("The website {website} is down. Status code {StatusCode}", website, result.StatusCode);
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
