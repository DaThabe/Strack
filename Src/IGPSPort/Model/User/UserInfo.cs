using UnitsNet;

namespace IGPSport.Model.User;

/// <summary>
/// 用户信息
/// </summary>
public class UserInfo
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long Id { get; set; }


    /// <summary>
    /// 头像网址
    /// </summary>
    public string AvatarUrl { get; set; } = string.Empty;

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 电话号
    /// </summary>
    public long PhoneNumber { get; set; }


    /// <summary>
    /// 性别
    /// </summary>
    public GenderType Gender { get; set; } = GenderType.Unknown;

    /// <summary>
    /// 身高
    /// </summary>
    public Length Height { get; set; }

    /// <summary>
    /// 体重
    /// </summary>
    public Mass Weight { get; set; }

    /// <summary>
    /// 粉丝数量
    /// </summary>
    public int Fans { get; set; }


    /// <summary>
    /// 生日
    /// </summary>
    public DateTimeOffset Birthday { get; set; } = DateTimeOffset.MinValue;

    /// <summary>
    /// 注册时间
    /// </summary>
    public DateTimeOffset RegisterTime { get; set; } = DateTimeOffset.MinValue;

    /// <summary>
    /// 最大心率
    /// </summary>
    public Frequency MHR { get; set; } = Frequency.Zero;

    /// <summary>
    /// 阈值心率
    /// </summary>
    public Frequency LTHR { get; set; } = Frequency.Zero;

    /// <summary>
    /// 阈值功率
    /// </summary>
    public Power FTP { get; set; } = Power.Zero;
}

//{
//  "code": 0,
//  "message": "",
//  "data": {
//    "strMemberId": "qOSqeybJi/TcSCMuVJTQdA==",
//    "memberId": 1027774,
//    "nickName": "DaThabe",
//    "avatar": "https://igp-zh.oss-cn-hangzhou.aliyuncs.com/1027774-9324e8e8-9597-496a-bbed-6c8985a1e72c",
//    "sex": 1,
//    "cityId": 166,
//    "cityName": "洛阳市",
//    "provinceName": "洛阳市",
//    "height": 180,
//    "weight": 61,
//    "birthDate": "2001-02-28",
//    "mhr": 203,
//    "lthr": 178,
//    "rideTime": 1379250,
//    "rideDistance": 8118863,
//    "rideCalorie": 288057,
//    "rideNum": 172,
//    "timeZone": 28800,
//    "bikeWeight": 10,
//    "bikeWheelSize": 2096,
//    "regTime": "10/13/2023 17:38:43",
//    "integral": 0,
//    "vO2max": 0,
//    "ftp": 175,
//    "attention": 8,
//    "fans": 6,
//    "unitMetric": 0,
//    "unitTemperature": 0,
//    "unitWeight": 0,
//    "unitHeight": 0,
//    "unitLength": 0,
//    "viewFriends": 0,
//    "deviceName": "iGS630,iGS520,BSC100S,BSC100,BSC200,BSC300,iGS630S,iGS800,BSC300T,BSC200S,BiNavi,WR02",
//    "hasPassword": true,
//    "phone": "18037902792",
//    "isOfficial": false,
//    "momentCount": 0,
//    "isIdentified": false,
//    "type": 1
//  }
//}