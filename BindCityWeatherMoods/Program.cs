var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddOptions<Config>()
    .Bind(builder.Configuration, options =>
    {
        //try not to use binding non public properties
        //options.BindNonPublicProperties= true;
        options.ErrorOnUnknownConfiguration = false;
    })
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IOptions<Config> _config;

    public Worker(ILogger<Worker> logger, IOptions<Config> config)
    {
        _logger = logger;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var city in _config.Value.Cities)
            {
                _logger.LogInformation("{CityName} is {CityWeatherMood}", city.Name, city.WeatherMood);
            }
            await Task.Delay(10000, stoppingToken);
        }
    }
}

public class Config
{
    [ValidateEnumeratedItems] public List<City> Cities { get; set; }
}

public class City
{
    [Required] public string Name { get; set; }
    [Required] public WeatherMood WeatherMood { get; set; }
}

public enum WeatherMood
{
    Sunny,
    Rainy,
    Cloudy
}