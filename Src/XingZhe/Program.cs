using Common;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using XingZhe;
using XingZhe.Model.Workout;

Console.Title = Assembly.GetExecutingAssembly().GetName().FullName;

var builder = Host.CreateDefaultBuilder(args)
    .UseCommon()
    .UseXingZhe()
    .Build();

await builder.StartAsync();


var clientService = builder.Services.GetXingZheClientProvider();

var client = clientService.Sessions.First().Client;
var gpx = await client.GetWorkoutGpxFileAsync(153465022);
var stream = await client.GetWorkoutStreamAsync(153465022);


var track = gpx.Tracks.FirstOrDefault();
if(track == null)
{
    track = new Common.Model.File.Gpx.Track()
    {
        Name = "运动轨迹",
        Points = []
    };
    gpx.Tracks.Add(track);
}

stream.AttachToTrackPoint(track.Points);


await builder.StopAsync();