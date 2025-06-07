using Common;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Xml.Linq;

Console.Title = Assembly.GetExecutingAssembly().GetName().FullName;

var builder = Host.CreateDefaultBuilder(args)
    .UseCommon()
    .Build();

await builder.StartAsync();


//var fitService = builder.Services.GetFitService();
//using var fs = File.OpenRead(@"C:\Users\datha\Desktop\1027774.fit");
//var fitFile = await fitService.ReadAsync(fs);

var gpxService = builder.Services.GetGpxService();

var document = XDocument.Load(@"C:\Users\datha\Desktop\146413915.gpx");
var fitFile = gpxService.Deserialize(document);

await builder.StopAsync();