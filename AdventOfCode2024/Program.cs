using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using AdventOfCode2024;
using AdventOfCode2024.Days;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

Env.Load();

IInputAgent inputAgent = new InputAgent(new HttpClient());
IResultPrinter resultPrinter = new ResultPrinter();
Day1 day1 = new Day1(inputAgent, resultPrinter);
for (int i = 0; i < 10000; i++)
{
await day1.Part1();
}
return;


var serviceCollection = new ServiceCollection();

// Build configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", false)
    .Build();

// Add logging
serviceCollection.AddLogging(config =>
{
    // clear out default configuration
    config.ClearProviders();

    config.AddConfiguration(configuration.GetSection("Logging"));
    config.AddConsole();
});

serviceCollection.AddHttpClient();
serviceCollection.AddScoped<IInputAgent, InputAgent>();
serviceCollection.AddScoped<IResultPrinter, ResultPrinter>();


var serviceProvider = serviceCollection.BuildServiceProvider();


// Use reflection to get all classes implementing IDay, Expect them to have the name Day<num> where num is a 1 or two digit number. Sort by the numeric value of that number. Then run Part1 and Part2 in order
IEnumerable<IDay?> days = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => t.GetInterfaces().Contains(typeof(IDay)))
    .OrderBy(t => int.Parse(Regex.Match(t.Name, @"\d+").Value))
    .Select(t => ActivatorUtilities.CreateInstance(serviceProvider, t) as IDay);

ILogger logger = serviceProvider.GetRequiredService<ILogger<Program>>();

var stopwatch = new Stopwatch();
var intervalStopwatch = new Stopwatch();

stopwatch.Start();
foreach (IDay? day in days)
{
    intervalStopwatch.Start();
    await day.Part1();
    intervalStopwatch.Stop();
    logger.LogDebug($"Day {day.GetType().Name} Part 1 took {intervalStopwatch.ElapsedMilliseconds}ms");
    intervalStopwatch.Reset();

    intervalStopwatch.Start();
    await day.Part2();
    intervalStopwatch.Stop();
    logger.LogDebug($"Day {day.GetType().Name} Part 2 took {intervalStopwatch.ElapsedMilliseconds}ms");
    intervalStopwatch.Reset();
}

stopwatch.Stop();
logger.LogInformation($"All days took {stopwatch.ElapsedMilliseconds}ms");