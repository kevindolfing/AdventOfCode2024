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

// get the day input parameter supplied by the the launch command
int? dayArg = args.Length > 0 ? int.Parse(args[0]) : null;

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

if (dayArg == null)
{
    // get class implementingIday containing the dayArg
    var dayType = Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => t.GetInterfaces().Contains(typeof(IDay)))
        .FirstOrDefault(t => t.Name == $"Day{DateTime.Now.Day}");

    if (dayType == null)
    {
        Console.WriteLine($"Day {DateTime.Now.Day} not found");
        return;
    }

    var day = ActivatorUtilities.CreateInstance(serviceProvider, dayType) as IDay;
    await day.Part1();
    await day.Part2();
    return;
}

if (dayArg == 0)
{
// Use reflection to get all classes implementing IDay, Expect them to have the name Day<num> where num is a 1 or two digit number. Sort by the numeric value of that number. Then run Part1 and Part2 in order
    IEnumerable<IDay?> days = Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => t.GetInterfaces().Contains(typeof(IDay)))
        .OrderBy(t => int.Parse(Regex.Match(t.Name, @"\d+").Value))
        .Select(t => ActivatorUtilities.CreateInstance(serviceProvider, t) as IDay);

    foreach (IDay? day in days)
    {
        await day.Part1();
        await day.Part2();
    }

    return;
}
else
{
    // get class implementing Iday containing the dayArg
    var dayType = Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => t.GetInterfaces().Contains(typeof(IDay)))
        .FirstOrDefault(t => t.Name == $"Day{dayArg}");

    if (dayType == null)
    {
        Console.WriteLine($"Day {dayArg} not found");
        return;
    }

    var day = ActivatorUtilities.CreateInstance(serviceProvider, dayType) as IDay;
    await day.Part1();
    await day.Part2();
}