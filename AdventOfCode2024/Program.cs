using System.Reflection;
using System.Text.RegularExpressions;
using AdventOfCode2024;
using AdventOfCode2024.Days;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;

Env.Load();

var serviceCollection = new ServiceCollection();

serviceCollection.AddHttpClient();
serviceCollection.AddScoped<InputAgent>();

var serviceProvider = serviceCollection.BuildServiceProvider();


// Use reflection to get all classes implementing IDay, Expect them to have the name Day<num> where num is a 1 or two digit number. Sort by the numeric value of that number. Then run Part1 and Part2 in order
var days = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => t.GetInterfaces().Contains(typeof(IDay)))
    .OrderBy(t => int.Parse(Regex.Match(t.Name, @"\d+").Value))
    .Select(t => ActivatorUtilities.CreateInstance(serviceProvider, t) as IDay);

foreach (var day in days)
{
    await day.Part1();
    await day.Part2();
}