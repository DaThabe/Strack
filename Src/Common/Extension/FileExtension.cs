using Common.Model.File.Fit;
using Common.Model.File.Gpx;
using Common.Service.File;
using System.Xml.Linq;
using UnitsNet;

namespace Common.Extension;



/// <summary>
/// 文件扩展
/// </summary>
public static class FileExtension
{
    public static GpxFile ToGpxFile(this FitFile fit)
    {
        GpxFile gpx = new();

        gpx.Metadata.Time = DateTimeOffset.Now;
        gpx.Metadata.AuthorName = "Strack";

        Track track = new() { Name = "运动轨迹" };

        foreach (var x in fit.Records)
        {
            if (x.Timestamp == null) continue;
            if (x.Longitude == null) continue;
            if (x.Latitude == null) continue;

            var point = new TrackPoint()
            {
                Longitude = x.Longitude.Value,
                Latitude = x.Latitude.Value,
                Altitude = x.Altitude,
                Time = x.Timestamp,
                Extension = new TrackPointExtension()
                {
                    Cadence = x.Cadence,
                    Distance = x.Distance,
                    Heartrate = x.Heartrate,
                    Power = x.Power,
                    Speed = x.Speed,
                    Temperature = x.Temperature
                }
            };

            track.Points.Add(point);
        }

        gpx.Tracks.Add(track);
        return gpx;
    }
}