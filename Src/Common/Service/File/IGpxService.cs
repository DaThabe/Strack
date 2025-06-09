using Common.Model.Exception;
using Common.Model.File.Gpx;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using UnitsNet;

namespace Common.Service.File;

public interface IGpxService
{
    /// <summary>
    /// 从 gpx 文本反序列化
    /// </summary>
    /// <param name="gpxSource"></param>
    GpxFile Deserialize(XDocument document);

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="gpx"></param>
    /// <returns></returns>
    XDocument Serialize(GpxFile gpx);
}

public class GpxService(ILogger<FitService> logger) : IGpxService
{
    public GpxFile Deserialize(XDocument document)
    {
        try
        {
            logger.LogTrace("正在解析 Gpx 文件");

            var gpxFile = MapperGpx(document);

            logger.LogTrace("Gpx 文件解析完成");
            return gpxFile;
        }
        catch (Exception ex)
        {
            throw new GpxException("Gpx 文件加载失败", ex);
        }
    }
    public XDocument Serialize(GpxFile model)
    {
        var node = MapperGpx(model);
        return new XDocument(new XDeclaration("1.0", "UTF-8", null), node);
    }




    //映射 文件信息
    private static GpxFile MapperGpx(XDocument document)
    {
        var nsManager = new XmlNamespaceManager(new NameTable());
        nsManager.AddNamespace("gpx", "http://www.topografix.com/GPX/1/1");

        //根节点
        var gpx = document.XPathSelectElement("/gpx:gpx", nsManager) ?? throw new ArgumentException("缺失 Gpx 节点");

        //文件信息
        var fileInfo = new Model.File.Gpx.FileInfo()
        {
            Version = gpx.Attribute("version")?.Value ?? "1.1",
            Creator = gpx.Attribute("creator")?.Value,
        };

        //元数据
        var metadataNode = gpx.XPathSelectElement("gpx:metadata", nsManager) ?? throw new ArgumentException("缺失 metadata 节点");
        var metadata = MapperMetadata(metadataNode, nsManager);

        //路径
        var trackNodes = gpx.XPathSelectElements("gpx:trk", nsManager) ?? throw new ArgumentException("缺失 trk 节点");
        var trks = trackNodes.Select(x => MapperTrack(x, nsManager)).ToList();


        return new GpxFile()
        {
            FileInfo = fileInfo,
            Metadata = metadata,
            Tracks = trks
        };
    }
    //映射 文件信息
    private static XElement MapperGpx(GpxFile model)
    {
        XNamespace ns = "http://www.topografix.com/GPX/1/1";
        XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        XElement node = new(ns + "gpx");

        node.Add(new XAttribute(XNamespace.Xmlns + "xsi", xsi));
        node.Add(new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"));
        node.Add(new XAttribute(xsi + "schemaLocation", "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd"));

        //文件信息
        if(!string.IsNullOrWhiteSpace(model.FileInfo.Version)) node.Add(new XAttribute("version", model.FileInfo.Version));
        if (!string.IsNullOrWhiteSpace(model.FileInfo.Creator)) node.Add(new XAttribute("creator", model.FileInfo.Creator));

        //元信息
        if (MapperMetadata(model.Metadata, ns) is XElement metadata) node.Add(metadata);

        //路径
        var trackNodes = model.Tracks.Select(x => MapperTrack(x, ns)).ToList();
        foreach (var i in trackNodes) node.Add(i);

        return node;
    }



    //映射元数据
    private static Metadata MapperMetadata(XElement metadata, XmlNamespaceManager nsm)
    {
        //名称
        var name = metadata?.XPathSelectElement("gpx:name", nsm)?.Value;
        //描述
        var desc = metadata?.XPathSelectElement("gpx:desc", nsm)?.Value;

        //作者
        var authorName = metadata?.XPathSelectElement("gpx:author/gpx:name", nsm)?.Value;
        //作者链接
        var authorLink = metadata?.XPathSelectElement("gpx:author/gpx:link", nsm)?.Attribute("href")?.Value;

        //关键字
        var keywords = metadata?.XPathSelectElement("gpx:keywords", nsm)?.Value?.Split([';', ',']);
        
        //时间 yyyy-MM-ddTHH:mm:ssZ
        var timeString = metadata?.XPathSelectElement("gpx:time", nsm)?.Value;
        var time = TryParserDateTime(timeString);

        return new Metadata
        {
            Name = name,
            AuthorName = authorName,
            AuthorLink = authorLink,
            Description = desc,
            Keywords = keywords,
            Timestamp = time
        };
    }
    //映射 metadata 节点
    private static XElement? MapperMetadata(Metadata model, XNamespace ns)
    {
        XElement node = new(ns + "metadata");

        //名称
        if (!string.IsNullOrWhiteSpace(model.Name)) node.Add(new XElement(ns + "name", model.Name));
        
        //描述
        if(!string.IsNullOrWhiteSpace(model.Description)) node.Add(new XElement(ns + "desc", model.Description));
        
        //时间
        if (model.Timestamp is DateTimeOffset time) node.Add(new XElement(ns + "time", time.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ")));

        //作者
        if (!string.IsNullOrWhiteSpace(model.AuthorName))
        {
            var authorNode = new XElement(ns + "author");
            authorNode.Add(new XElement(ns + "name", model.AuthorName));

            if(!string.IsNullOrWhiteSpace(model.AuthorLink))
            {
                authorNode.Add(new XElement(ns + "link",
                    new XAttribute("href", model.AuthorLink),
                    new XElement(ns + "text", model.AuthorName)));
            }

            node.Add(authorNode);
        }

        //关键字
        if (model.Keywords is not null)
        {
            var keyword = string.Join(",", model.Keywords);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                node.Add(new XElement(ns + "keywords", keyword));
            }
        }


        if (!node.Elements().Any()) return null;
        return node;
    }


    //映射 轨迹
    private static Track MapperTrack(XElement trk, XmlNamespaceManager nsm)
    {
        var name = trk?.XPathSelectElement("gpx:name", nsm)?.Value;

        var trkpts = trk?.XPathSelectElements("gpx:trkseg/gpx:trkpt", nsm)
            .Select(trkptNode => MapperTrackPoint(trkptNode, nsm))
            .ToList();

        return new Track
        {
            Name = name,
            Points = trkpts ?? []
        };
    }
    //映射 trk 节点
    private static XElement? MapperTrack(Track model, XNamespace ns)
    {
        XElement node = new(ns + "trk");

        if (!string.IsNullOrWhiteSpace(model.Name)) node.Add(new XElement(ns + "name", model.Name));

        var points = model.Points.Select(x => MapperTrackPoint(x, ns)).ToList();
        if (points.Count > 0) node.Add(new XElement(ns + "trkseg", points));

        return node;
    }


    //映射 轨迹点
    private static TrackPoint MapperTrackPoint(XElement trkpt, XmlNamespaceManager nsm)
    {
        //时间
        var timeString = trkpt.XPathSelectElement("gpx:time", nsm)?.Value;
        var time = TryParserDateTime(timeString);

        //经度
        var lonString = trkpt.Attribute("lon")?.Value ?? "0.0";
        var lon = double.Parse(lonString);

        //维度
        var latString = trkpt.Attribute("lat")?.Value ?? "0.0";
        var lat = double.Parse(latString);

        //海拔
        var altitudeString = trkpt.XPathSelectElement("gpx:ele", nsm)?.Value ?? "0.0";
        var altitude = double.Parse(altitudeString);



        ////扩展数据
        //var extensions = trkpt.XPathSelectElement("extensions");
        ////速度
        //var speedString = extensions?.XPathSelectElement("speed")?.Value;
        ////踏频
        //var cadenceString = extensions?.XPathSelectElement("cadence")?.Value;
        ////心率
        //var heartrateString = extensions?.XPathSelectElement("heartrate")?.Value;
        ////功率
        //var powerString = extensions?.XPathSelectElement("power")?.Value;

        return new TrackPoint
        {
            Timestamp = time ?? DateTimeOffset.MinValue, //TODO: 没有时间的采样点等待解决
            Longitude = lon,
            Latitude = lat,
            Altitude = Length.FromMeters(altitude)
        };
    }
    //映射 trkpt 节点
    private static XElement MapperTrackPoint(TrackPoint model, XNamespace ns)
    {
        XElement node = new(ns + "trkpt");

        //基础属性
        node.Add(new XAttribute("lon", Math.Round(model.Longitude, 5)));
        node.Add(new XAttribute("lat", Math.Round(model.Latitude, 5)));
        node.Add(new XElement(ns + "time", model.Timestamp.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ")));
        if (model.Altitude is Length len) node.Add(new XElement(ns + "ele", Math.Round(len.Meters, 1)));

        //扩展属性
        List<XElement> extensionElements = [];

        if (model.Speed is Speed speed) extensionElements.Add(new XElement(ns + "speed", (int)speed.MetersPerSecond));
        if (model.Cadence is Frequency cadence) extensionElements.Add(new XElement(ns + "cadence", (int)cadence.CyclesPerMinute));
        if (model.Heartrate is Frequency heartrate) extensionElements.Add(new XElement(ns + "heartrate", (int)heartrate.BeatsPerMinute));
        if (model.Power is Power power) extensionElements.Add(new XElement(ns + "power", (int)power.Watts));
        if (model.Temperature is Temperature temperature) extensionElements.Add(new XElement(ns + "temperature", Math.Round(temperature.DegreesCelsius, 1)));

        //添加扩展属性
        if (extensionElements.Count > 0)
        {
            XElement extensions = new(ns + "extensions", extensionElements);
            node.Add(extensions);
        }

        return node;
    }


    //尝试转换时间 (yyyy-MM-ddTHH:mm:ssZ)
    private static DateTimeOffset? TryParserDateTime(string? timeString)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(timeString)) return null;
            var dateTime = DateTimeOffset.ParseExact(timeString, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            return dateTime;
        }
        catch
        {
            return null;
        }
    }


    
}


public static class FUck
{
    /// <summary>
    /// 从文件加载Gpx
    /// </summary>
    /// <param name="service"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static async Task<GpxFile> LoadFromPathAsync(this IGpxService service, string filePath)
    {
        if (!System.IO.File.Exists(filePath)) throw new FileNotFoundException($"Gpx文件不存在:{filePath}", filePath);

        var gpxContent = await System.IO.File.ReadAllTextAsync(filePath);
        var document = XDocument.Parse(gpxContent);
        return service.Deserialize(document);
    }

    /// <summary>
    /// 保存到文件
    /// </summary>
    /// <param name="service"></param>
    /// <param name="gpx"></param>
    /// <param name="folder"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static async Task SaveAsync(this IGpxService service, GpxFile gpx, string folder, string fileName)
    {
        if (!string.IsNullOrWhiteSpace(folder))
        {
            Directory.CreateDirectory(folder);
        }
        var filePath = Path.Combine(folder, fileName);

        using var fs = System.IO.File.OpenWrite(filePath);
        await service.Serialize(gpx).SaveAsync(fs, SaveOptions.None, default);
    }
}