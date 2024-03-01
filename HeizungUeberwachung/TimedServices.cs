using HeizungUeberwachung.ServiceDto;

namespace HeizungUeberwachung;

public class TimedServices : IHostedService, IDisposable
{
    private Timer? timer = null;
    private readonly IServiceProvider serviceProvider;
    private readonly IConfiguration configuration;
    private State state = State.On;
    private const double turnOffTemperature = 52;
    private const double turnOnTemp = 55;

    public TimedServices(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        this.serviceProvider = serviceProvider;
        this.configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        timer = new Timer(DoWork, null, 0, 10_000); // prod 600_000

        return Task.CompletedTask;
    }

    private void DoWork(object? _)
    {
        // Local implementation of previous implementation to be replaced.
        // d=gs2988 --> prod
        Status status;
        using (var scope = serviceProvider.CreateScope())
        {
            var manager = scope.ServiceProvider.GetRequiredService<Manager>();
            status = manager.GetStatus();
        }

        if (state == State.On && status.Temperature < turnOffTemperature)
        {
            var apiKey = configuration["PushSaferApiKey"];
            var group = configuration["PushSaferGroup"];
            state = State.Off;
            var client = new HttpClient();
            client.GetAsync(@$"http://www.pushsafer.com/api?k={apiKey}&d={group}&i=5&c=%23ffcccc&v=1&m=Vorlauf%20kuehlt%20ab%3A%20Temperatur%3A%20{(int)status.Temperature}%C2%B0C");
        }
        else if (state == State.Off && status.Temperature > turnOnTemp)
        {
            var apiKey = configuration["PushSaferApiKey"];
            var group = configuration["PushSaferGroup"];
            state = State.On;
            var client = new HttpClient();
            client.GetAsync($"http://www.pushsafer.com/api?k={apiKey}&d={group}&i=4&m=Vorlauftemperatur%20normal");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }
    public void Dispose()
    {
        timer?.Dispose();
    }

}
