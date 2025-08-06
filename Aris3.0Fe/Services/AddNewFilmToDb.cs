
using System.Net.Http;

namespace Aris3._0Fe.Services
{
    public class AddNewFilmToDb : BackgroundService
    {
        private readonly ILogger<AddNewFilmToDb> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceProvider _services;
        private Timer _timer;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(30);
        private bool _hasRunOnce = false;
        public AddNewFilmToDb(ILogger<AddNewFilmToDb> logger, IHttpClientFactory httpClientFactory, IServiceProvider services)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _services = services;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
          
            _timer = new Timer(async _ => await DoWorkAsync(), null, TimeSpan.Zero, _interval);
            return Task.CompletedTask;
        }

        private async Task DoWorkAsync()
        {
            try
            {
                if (!_hasRunOnce || DateTime.UtcNow - _lastRun > _interval)
                {
                    _hasRunOnce = true;
                    _lastRun = DateTime.UtcNow;

                    var httpClient = _httpClientFactory.CreateClient();
                    var response = await httpClient.PostAsync("https://localhost:7248/api/film/4", null);

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Film sync succeeded at: {time}", DateTimeOffset.Now);
                    }
                    else
                    {
                        _logger.LogWarning("Film sync failed with status code: {code}", response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during film sync.");
            }
        }

        private DateTime _lastRun = DateTime.MinValue;

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
