using UnitsNet;

namespace IGPSport.Model.Activity;

/// <summary>
/// 单次训练简略信息
/// </summary>
public class ActivitySummary
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Fit 文件网址
    /// </summary>
    public string FitFileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 时间
    /// </summary>
    public DateTimeOffset StartTime { get; set; } = DateTimeOffset.MinValue;

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 类型
    /// </summary>
    public ActivityType Type { get; set; } = ActivityType.All;


    /// <summary>
    /// 均速
    /// </summary>
    public Speed AvgSpeed { get; set; } = Speed.Zero;

    /// <summary>
    /// 持续时间
    /// </summary>
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// 距离
    /// </summary>
    public Length Distance { get; set; } = Length.Zero;



    public override string ToString()
    {
        return $"[{StartTime:yyyy-MM-dd HH:MM}] 类型:{Type}, Id: {Id}, 标题:{Title}, 均速:{AvgSpeed}, 用时:{Duration}, 距离:{Distance}";
    }
}


//{
//  "id": "683d765a567c8ff04922c67a",
//  "rideId": 31827705,
//  "exerciseType": 0,
//  "title": "户外骑行",
//  "startTime": "2025.06.02",
//  "rideDistance": 77816.56,
//  "totalMovingTime": 13112,
//  "avgSpeed": 5.934,
//  "dataStatus": 1,
//  "errorType": 0,
//  "analysisStatus": 1,
//  "fitOssPath": "https://igp-zh.oss-cn-hangzhou.aliyuncs.com/1027774-d3bf782a-73ee-4cbc-a2db-38715d663dc3",
//  "label": 1,
//  "isOpen": 0,
//  "unRead": false
//}