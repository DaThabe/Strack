using Common;
using Common.Model.File.Fit;
using Common.Model.File.Gpx;
using IGPSport;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Xml.Linq;

Console.Title = Assembly.GetExecutingAssembly().GetName().FullName;

var builder = Host.CreateDefaultBuilder(args)
    .UseCommon()
    .UseIGPSport()
    .Build();

await builder.StartAsync();


var clientProvider = builder.Services.GetIGPSportClientProvider();
var gpxService = builder.Services.GetGpxService();

var client = clientProvider.Sessions.First().Client;

await foreach(var i in client.GetActivitySummariesAsync())
{
    if (File.Exists(Path.Combine("IGPSport", $"{i.Id}.gpx"))) continue;

    var fitFile = await client.GetActivityFitFileAsync(i.FitFileUrl);
    var gpx = ToGpxFile(fitFile);

    await SaveGpxFile("IGPSport", $"{i.Id}.gpx", gpx);
    
    Console.WriteLine($"{i.Id} 下载完成");
    await Task.Delay(100);
}

await builder.StopAsync();

async Task SaveGpxFile(string folder, string fileName, GpxFile gpx)
{
    Directory.CreateDirectory(folder);
    var filePath = Path.Combine(folder, fileName);

    using var fs = File.OpenWrite(filePath);
    await gpxService.Serialize(gpx).SaveAsync(fs, SaveOptions.None, default);
}

static GpxFile ToGpxFile(FitFile fit)
{
    GpxFile gpx = new();

    gpx.Metadata.Timestamp = DateTimeOffset.Now;
    gpx.Metadata.AuthorName = "Strack.IGPSport";

    Track track = new() { Name = "运动轨迹" };

    foreach(var x in fit.Records)
    {
        if (x.Timestamp == null) continue;
        if (x.Longitude == null) continue;
        if (x.Latitude == null) continue;

        var point = new TrackPoint()
        {
            Timestamp = x.Timestamp.Value,
            Longitude = x.Longitude.Value,
            Latitude = x.Latitude.Value,
            Altitude = x.Altitude,
            Cadence = x.Cadence,
            Distance = x.Distance,
            Heartrate = x.Heartrate,
            Power = x.Power,
            Speed = x.Speed,
            Temperature = x.Temperature,
        };

        track.Points.Add(point);
    }

    gpx.Tracks.Add(track);
    return gpx;
}