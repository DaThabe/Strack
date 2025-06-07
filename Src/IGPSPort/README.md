# 行者 SDK

## API

### 获取用户信息

> [GET] https://prod.zh.igpsport.com/service/mobile/api/User/UserInfo

**响应格式**

```json
{
  "code": 0,
  "message": "",
  "data": {
    "strMemberId": "qOSqeybJi/TcSCMuVJTQdA==",
    "memberId": 1027774,
    "nickName": "DaThabe",
    "avatar": "https://igp-zh.oss-cn-hangzhou.aliyuncs.com/1027774-9324e8e8-9597-496a-bbed-6c8985a1e72c",
    "sex": 1,
    "cityId": 166,
    "cityName": "洛阳市",
    "provinceName": "洛阳市",
    "height": 180,
    "weight": 61,
    "birthDate": "2001-02-28",
    "mhr": 203,
    "lthr": 178,
    "rideTime": 1379250,
    "rideDistance": 8118863,
    "rideCalorie": 288057,
    "rideNum": 172,
    "timeZone": 28800,
    "bikeWeight": 10,
    "bikeWheelSize": 2096,
    "regTime": "10/13/2023 17:38:43",
    "integral": 0,
    "vO2max": 0,
    "ftp": 175,
    "attention": 8,
    "fans": 6,
    "unitMetric": 0,
    "unitTemperature": 0,
    "unitWeight": 0,
    "unitHeight": 0,
    "unitLength": 0,
    "viewFriends": 0,
    "deviceName": "iGS630,iGS520,BSC100S,BSC100,BSC200,BSC300,iGS630S,iGS800,BSC300T,BSC200S,BiNavi,WR02",
    "hasPassword": true,
    "phone": "18037902792",
    "isOfficial": false,
    "momentCount": 0,
    "isIdentified": false,
    "type": 1
  }
}
```

### 获取所有活动简要

> [GET] https://prod.zh.igpsport.com/service/web-gateway/web-analyze/activity/queryMyActivity?pageNo=1&pageSize=20&reqType=0&sort=1

| 参数 | 类型 | 描述 |
| :-: | :-: |  :-: |
| pageNo | int | 分页号 |
| pageSize | int | 分页数量 |
| sport | enum | 运动类型 |
| reqType | int | 请求类型? (不知道干嘛的默认是1) |

**运动类型枚举** 

> 全部 = 0   
> 骑行 = 1  
> 跑步 = 2  

**响应格式**

```json
{
  "code": 0,
  "message": "success",
  "data": {
    "pageNo": 1,
    "pageSize": 20,
    "totalPage": 15,
    "totalRows": 298,
    "rows": [
      {
        "id": "564s4da564d56as484",
        "rideId": 31827705,
        "exerciseType": 0,
        "title": "户外骑行",
        "startTime": "2025.06.02",
        "rideDistance": 77816.56,
        "totalMovingTime": 13112,
        "avgSpeed": 5.934,
        "dataStatus": 1,
        "errorType": 0,
        "analysisStatus": 1,
        "fitOssPath": "https://igp-zh.oss-cn-hangzhou.aliyuncs.com/1027774-abc123-acb-edf-ghi-123123123s4s5s",
        "label": 1,
        "isOpen": 0,
        "unRead": false
      },
      ...
    ]
  }
}
```

### 获取活动明细

> 
https://prod.zh.igpsport.com/service/web-gateway/web-analyze/activity/queryActivityDetail/{activityId}

| 参数 | 类型 | 描述 |
| :-: | :-: |  :-: |
| \{activityId\} | long | 活动Id |

**响应格式**

```json
{
  "code": 0,
  "message": "success",
  "data": {
    "rideId": 31827705,
    "memberId": 1027774,
    "title": "户外骑行",
    "product": 301,
    "softwareVersion": "1.48",
    "avgSpeed": 5.934,
    "avgMovingSpeed": 5.934,
    "maxSpeed": 11.27,
    "startTimeWithWeek": "2025.06.2 星期一 13:56",
    "startTime": "2025-06-02 13:56:49",
    "movingTime": 13112,
    "endTime": "2025-06-02 17:58:43",
    "totalTime": 14514,
    "rideDistance": 77816,
    "totalAscent": 724,
    "minHrm": 88,
    "avgHrm": 143,
    "maxHrm": 173,
    "avgCad": 84,
    "maxCad": 125,
    "avgAltitude": 290,
    "minAltitude": 152,
    "maxAltitude": 451,
    "calorie": 2742,
    "upslopeMaxGrade": 12.34,
    "downslopeMaxGrade": -11.04,
    "upslopeAvgVerticalSpeed": 467,
    "upslopeMaxVerticalSpeed": 1447,
    "downslopeAvgVerticalSpeed": -745,
    "downslopeMaxVerticalSpeed": -2282,
    "avgTemperature": 32,
    "maxTemperature": 37,
    "upslopeDistance": 31754,
    "upslopeAvgGrade": 2.28,
    "downslopeDistance": 31371,
    "totalDescent": 709,
    "downslopeAvgGrade": -2.26,
    "exerciseType": 0,
    "manufacturer": 115,
    "avgBalance": [
      null,
      null
    ],
    "avgTQEffect": [
      null,
      null
    ],
    "avgPedSmooth": [
      null,
      null
    ],
    "avgPco": [
      null,
      null
    ],
    "spZoom": [
      {
        "grade": 1,
        "zoom": "0-18",
        "startNum": 0,
        "endNum": 18,
        "percentage": 34.65,
        "time": 4543
      },
      {
        "grade": 2,
        "zoom": "18-25",
        "startNum": 18,
        "endNum": 25,
        "percentage": 34.91,
        "time": 4577
      },
      {
        "grade": 3,
        "zoom": "25-30",
        "startNum": 25,
        "endNum": 30,
        "percentage": 18.53,
        "time": 2429
      },
      {
        "grade": 4,
        "zoom": "30-35",
        "startNum": 30,
        "endNum": 35,
        "percentage": 8.53,
        "time": 1118
      },
      {
        "grade": 5,
        "zoom": "35-999",
        "startNum": 35,
        "endNum": 999,
        "percentage": 3.39,
        "time": 444
      }
    ],
    "cadZoom": [
      {
        "grade": 1,
        "zoom": "0-60",
        "startNum": 0,
        "endNum": 60,
        "percentage": 6.39
      },
      {
        "grade": 2,
        "zoom": "60-80",
        "startNum": 60,
        "endNum": 80,
        "percentage": 20.71
      },
      {
        "grade": 3,
        "zoom": "80-100",
        "startNum": 80,
        "endNum": 100,
        "percentage": 65.09
      },
      {
        "grade": 4,
        "zoom": "100-120",
        "startNum": 100,
        "endNum": 120,
        "percentage": 7.79
      },
      {
        "grade": 5,
        "zoom": "120-160",
        "startNum": 120,
        "endNum": 160,
        "percentage": 0.02
      },
      {
        "grade": 6,
        "zoom": "160-255",
        "startNum": 160,
        "endNum": 255,
        "percentage": 0
      }
    ],
    "hrZoom": [
      {
        "grade": 1,
        "zoom": "0-119",
        "startNum": 0,
        "endNum": 119,
        "percentage": 10.97,
        "time": 1438
      },
      {
        "grade": 2,
        "zoom": "119-144",
        "startNum": 119,
        "endNum": 144,
        "percentage": 29.82,
        "time": 3909
      },
      {
        "grade": 3,
        "zoom": "144-167",
        "startNum": 144,
        "endNum": 167,
        "percentage": 56.05,
        "time": 7349
      },
      {
        "grade": 4,
        "zoom": "167-178",
        "startNum": 167,
        "endNum": 178,
        "percentage": 3.16,
        "time": 414
      },
      {
        "grade": 5,
        "zoom": "178-203",
        "startNum": 178,
        "endNum": 203,
        "percentage": 0,
        "time": 0
      }
    ],
    "upAndDownSlopeZoom": [
      {
        "type": 0,
        "distance": 31371.68,
        "percentage": 0.403
      },
      {
        "type": 2,
        "distance": 31754.38,
        "percentage": 0.408
      },
      {
        "type": 1,
        "distance": 14690.5,
        "percentage": 0.189
      }
    ],
    "fitUrl": "https://igp-zh.oss-cn-hangzhou.aliyuncs.com/1027774-d3bf782a-73ee-4cbc-a2db-38715d663dc3",
    "status": 1,
    "errorType": 0,
    "openStatus": 0,
    "productName": "iGPSPORT BSC300",
    "dataSyncStravaStatus": 1,
    "deviceInfo": {
      "deviceName": "iGPSPORT BSC300",
      "deviceImage": "https://igp-zh.oss-cn-hangzhou.aliyuncs.com/83a2a9f9-a8a8-4f14-bf30-26574b719af6",
      "softwareVersion": "1.48"
    },
    "fitFixed": false,
    "hideStartAndEnd": true,
    "hideStartAndEndDistance": 1000,
    "label": 1
  }
}
```

### 获取活动分段

> https://prod.zh.igpsport.com/service/web-gateway/web-analyze/activity/queryActivityLap/{activityId}

| 参数 | 类型 | 描述 |
| :-: | :-: |  :-: |
| \{activityId\} | long | 活动Id |

**响应格式**

```json
{
  "code": 0,
  "message": "success",
  "data": [
    {
      "lap": "1",
      "times": 3491,
      "distance": 21655,
      "avgSpeed": 6.203000068664551,
      "differ": 0,
      "maxSpeed": 10.468000411987305,
      "avgCad": 88,
      "maxCad": 115,
      "avgHr": 146,
      "maxHr": 168,
      "avgAlt": 187,
      "maxAlt": 217,
      "avgTemperature": 33,
      "maxTemperature": 36,
      "totalCalories": 755,
      "avgPosGrade": 0.96,
      "maxPosGrade": 4.23,
      "avgNegGrade": -0.97,
      "maxNegGrade": -5.94,
      "avgPosVerticalSpeed": 288,
      "avgNegVerticalSpeed": -428.40002,
      "totalAscent": 90,
      "totalDescent": 43,
      "avgPace": 161
    },
    {
      "lap": "2",
      "times": 4156,
      "distance": 20072,
      "avgSpeed": 4.828999996185303,
      "differ": 0,
      "maxSpeed": 10.694999694824219,
      "avgCad": 82,
      "maxCad": 113,
      "avgHr": 146,
      "maxHr": 173,
      "avgAlt": 339,
      "maxAlt": 451.6000061035156,
      "avgTemperature": 33,
      "maxTemperature": 37,
      "totalCalories": 895,
      "avgPosGrade": 3.35,
      "maxPosGrade": 12.34,
      "avgNegGrade": -3.52,
      "maxNegGrade": -11.04,
      "avgPosVerticalSpeed": 522,
      "avgNegVerticalSpeed": -1065.6,
      "totalAscent": 434,
      "totalDescent": 188,
      "avgPace": 207
    },
    {
      "lap": "3",
      "times": 2338,
      "distance": 16080,
      "avgSpeed": 6.876999855041504,
      "differ": 0,
      "maxSpeed": 11.270000457763672,
      "avgCad": 82,
      "maxCad": 113,
      "avgHr": 139,
      "maxHr": 171,
      "avgAlt": 392.6000061035156,
      "maxAlt": 451.79998779296875,
      "avgTemperature": 31,
      "maxTemperature": 32,
      "totalCalories": 462,
      "avgPosGrade": 2.16,
      "maxPosGrade": 9.37,
      "avgNegGrade": -1.76,
      "maxNegGrade": -6.48,
      "avgPosVerticalSpeed": 450,
      "avgNegVerticalSpeed": -558,
      "totalAscent": 89,
      "totalDescent": 183,
      "avgPace": 145
    },
    {
      "lap": "4",
      "times": 3127,
      "distance": 20008,
      "avgSpeed": 6.3979997634887695,
      "differ": 0,
      "maxSpeed": 10.152000427246094,
      "avgCad": 84,
      "maxCad": 125,
      "avgHr": 140,
      "maxHr": 169,
      "avgAlt": 262.3999938964844,
      "maxAlt": 356.20001220703125,
      "avgTemperature": 30,
      "maxTemperature": 31,
      "totalCalories": 629,
      "avgPosGrade": 2.09,
      "maxPosGrade": 6.21,
      "avgNegGrade": -2.64,
      "maxNegGrade": -7.18,
      "avgPosVerticalSpeed": 467.99997,
      "avgNegVerticalSpeed": -817.2,
      "totalAscent": 110,
      "totalDescent": 293,
      "avgPace": 156
    },
    {
      "lap": "总计",
      "times": 13112,
      "distance": 77816,
      "avgSpeed": 5.934000015258789,
      "differ": 0,
      "maxSpeed": 11.270000457763672,
      "avgCad": 84,
      "maxCad": 125,
      "avgHr": 143,
      "maxHr": 173,
      "avgAlt": 290.429443359375,
      "maxAlt": 451.79998779296875,
      "avgTemperature": 32,
      "maxTemperature": 37,
      "normalizedPower": 0,
      "totalCalories": 2742,
      "avgPosGrade": 2.28,
      "maxPosGrade": 12.34,
      "avgNegGrade": -2.26,
      "maxNegGrade": -11.04,
      "avgPosVerticalSpeed": 467.99997,
      "avgNegVerticalSpeed": -745.2,
      "totalAscent": 724,
      "totalDescent": 709,
      "avgPace": 168
    }
  ]
}
```

### 活动活动 Fit 文件

> {fitUrl} 从简要中可以获取到

**响应格式**

```Fit
.Fit 二进制格式 (还不知道怎么读取)
```