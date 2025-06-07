# ���� SDK

## API

### ��ȡ�û���Ϣ

> [GET] https://www.imxingzhe.com/api/v1/user/user_info/

**��Ӧ��ʽ**

```json
{
    "code": 0,
    "msg": "",
    "data": {
        "id": 6117373,
        "avatar": "https://static.imxingzhe.com/photo/6117373/xingzhe/1681361608338.jpg",
        "username": "DaThabe",
        "hots": 8678,
        "mobile": "18037902792",
        "city_id": 166,
        "province": "\u6cb3\u5357\u7701",
        "sex": 1,
        "height": 180,
        "weight": 57.0,
        "birthday": "1999-01-01",
        "skin": "",
        "skin_color": "",
        "license_number": "",
        "medal_small": [
            {
                "url": "http://static.imxingzhe.com/medal/67ba716f41dc089a77de9acab2e7a05c.png",
                "mlevel": 0,
                "mid": 78
            }
        ],
        "is_pro": false,
        "pro_stime": 1681246959000,
        "pro_etime": 1691787759000,
        "pro_name": "",
        "ulevel": 9,
        "main_team": "\u5915\u9633\u7ea2\u83dc\u817f\u4ff1\u4e50\u90e8:194765",
        "date_joined": "2023-03-26 13:39:08",
        "can_edit_sex": true,
        "can_edit_birthday": true
    }
}
```

### ��ȡ����ѵ����Ҫ

> [GET] https://www.imxingzhe.com/api/v1/pgworkout/?offset=0&limit=24&sport=&year=&month=

| ���� | ���� | ���� |
| :-: | :-: |  :-: |
| offset | int | ƫ���� > 0 |
| limit | int | ÿ�λ�ȡ���� |
| sport | enum | �˶����� |
| year | int | ɸѡ��� |
| month | int | ɸѡ�·� |

**�˶�����ö��**  
> ���� = 0  
> ͽ�� = 1  
> �ܲ� = 2  
> ���� = 3  
> ��Ӿ = 5  
> ��ѩ = 6  
> ���� = 8  
> �������� = 11  
> �������� = 12  

**��Ӧ��ʽ**

```json
{
    "code": 0,
    "data": {
        "data": [
            {
                "id": 189731154,
                "uuid": "106119fb-e4b9-48bf-91f5-78a6682f738a",
                "sport": 5,
                "title": "2025-05-14 �賿 ��Ӿ",
                "duration": 26,
                "distance": 5,
                "elevation_gain": 0,
                "start_time": 1747163233000,
                "thumbnail": "https://static.imxingzhe.com/workout/106119fb-e4b9-48bf-91f5-78a6682f738a.png!workoutThumb",
                "tss": null,
                "loc_source": 1,
                "has_cadence": false,
                "has_heartrate": false,
                "has_power": false,
                "credits": 15,
                "avg_speed": 0.69,
                "hidden": 0
            },
            ...
    },
    "msg": ""
}
```

### ��ȡѵ����ϸ

> https://www.imxingzhe.com/api/v1/pgworkout/{workoutId}/?segments=true&slopes=true&pois=true&laps=true

| ���� | ���� | ���� |
| :-: | :-: |  :-: |
| \{workoutId\} | long | ѵ��Id |
| segments | bool | ����� (T or F) ��ûӰ�� |
| slopes | bool | ����� (T or F) ��ûӰ�� |
| pois | bool | ����� (T or F) ��ûӰ�� |
| laps | bool | ����� (T or F) ��ûӰ�� |

**��Ӧ��ʽ**

```json

{
    "code": 200,
    "data": {
        "workout": {
            "title": "2023-11-27 ���� ����",
            "id": 146413915,
            "sport": 3,
            "like_count": 0,
            "comment_count": 0,
            "workout_id": 146413915,
            "avg_speed": 23.58,
            "max_speed": 37.33,
            "hidden": 0,
            "duration": 1491,
            "is_valid": 1,
            "is_merge": false,
            "calories": 234000,
            "start_time": 1701045035000,
            "end_time": 1701046796000,
            "distance": 9767,
            "uuid": "12123123123-546456-1sdasd-dasdas545",
            "thumbnail": "https://static.imxingzhe.com/workout/810a6197-22c3-49e5-ac4d-d60de882e9b6.png",
            "point_counts": 1496,
            "elevation_gain": 13,
            "elevation_loss": 14,
            "detail": "",
            "credits": 11,
            "cities": {
                "cities": [
                    9901
                ]
            },
            "segments_km": [],
            "threed_workout": "",
            "user": 6117373,
            "loc_source": 14,
            "other_info": {},
            "grade_data": {
                "down_distance": 345,
                "down_duration": 52,
                "flat_distance": 9015,
                "flat_duration": 1376,
                "up_distance": 405,
                "up_duration": 52,
                "vam": 0,
                "max_altitude": 127,
                "avg_altitude": 65036
            },
            "upload_time": "2023-11-27 21:00:36",
            "equipment_info": [
                {
                    "manufacturer": "Dabuziduo",
                    "product": 16,
                    "serialNumber": 896613,
                    "timeCreated": "2023-11-27T00:30:35Z",
                    "number": 2748,
                    "productName": "G N6",
                    "hardware_version": 12,
                    "software_version": 109,
                    "name": "����СG+(Gen.2)",
                    "name_cn": "����СG+(Gen.2)",
                    "vendor": "�Ϻ����Զ���Ϣ�Ƽ����޹�˾"
                }
            ],
            "desc": "",
            "source": "Sony , XQ-BE72 , 13 , 3.20.3",
            "weather": {
                "avg_temp": 12,
                "min_temp": 0,
                "max_temp": 9
            },
            "is_fit": true,
            "segments_hr": "",
            "segments_ca": "",
            "min_grade": -11,
            "max_grade": 6,
            "avg_grade": 0,
            "max_cadence": 120,
            "avg_cadence": 89,
            "max_heartrate": 0,
            "avg_heartrate": 0,
            "hr_max": 200,
            "hr_threshold": 172,
            "power_max": 0,
            "power_avg": 0,
            "power_np": 0.0,
            "power_if": 0.0,
            "power_vi": 0.0,
            "power_tss": 0.0,
            "power_ftp": 200.0,
            "heart_source": 0,
            "cadence_source": 2,
            "power_source": 0,
            "segments_power": 0,
            "max_altitude": 127,
            "avg_altitude": 65036,
            "is_like": 0,
            "validation": {},
            "is_tag_cvp": false
        },
        "user": {
            "userid": 6117373,
            "username": "DaThabe",
            "avatar": "https://static.imxingzhe.com/photo/6117373/xingzhe/1681361608338.jpg",
            "medal_small": [
                {
                    "url": "http://static.imxingzhe.com/medal/67ba716f41dc089a77de9acab2e7a05c.png",
                    "mlevel": 0,
                    "mid": 78
                }
            ],
            "ulevel": 9,
            "main_team": "Ϧ������Ⱦ��ֲ�:194765",
            "license_number": "",
            "skin": "",
            "skin_color": "",
            "pro_name": "���",
            "pro_stime": 1681246959000,
            "pro_etime": 1691787759000,
            "is_pro": false,
            "ftp": 190.0,
            "lthr": 184,
            "max_hr": 201,
            "weight": 57
        },
        "slopes": [],
        "segments": [],
        "pois": [],
        "laps": []
    },
    "msg": ""
}
```

### ��ȡѵ���������Ҫ

> [GET] https://www.imxingzhe.com/api/v1/pgworkout/{workoutId}/stream/

| ���� | ���� | ���� |
| :-: | :-: |  :-: |
| workoutId | long | ѵ��Id |

**��Ӧ��ʽ**

```json
{
    "code": 0,
    "data": {
        altitude : [ 1, 2, 3, ... ],
        cadence :  [ 1, 2, 3, ... ],
        distance :  [ 1, 2, 3, ... ],
        heartrate :  [ 1, 2, 3, ... ],
        left_balance :  [ 1, 2, 3, ... ],
        power :  [ 1, 2, 3, ... ],
        right_balance :  [ 1, 2, 3, ... ],
        left_balance :  [ 1, 2, 3, ... ],
        speed :  [ 1, 2, 3, ... ],
        temperature :  [ 1, 2, 3, ... ],
        timestamp :  [ 1, 2, 3, ... ],
        location :  [ [1, 2], [1,2], ... ],
    },
    "msg": ""
}
```

### ��ȡѵ���켣

> [GET] https://www.imxingzhe.com/api/v1/pgworkout/{workoutId}/gpx/

| ���� | ���� | ���� |
| :-: | :-: |  :-: |
| workoutId | long | ѵ��Id |

**��Ӧ��ʽ**

```xml
<?xml version="1.0" encoding="UTF-8"?>
<gpx xmlns="http://www.topografix.com/GPX/1/1" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd" version="1.1" creator="gpx.py -- https://github.com/tkrajina/gpxpy">
  <metadata>
    <name>�����������</name>
    <desc>�����������</desc>
    <author>
      <name>�����������</name>
      <link href="http://www.imxingzhe.com">
        <text>�����������</text>
      </link>
    </author>
    <keywords>�����������;�켣</keywords>
  </metadata>
  <trk>
    <name>�����������</name>
    <trkseg>
      <trkpt lat="34.678851" lon="112.460861">
        <time>2023-11-27T08:32:40Z</time>
      </trkpt>
      ...
    </trkseg>
  </trk>
</gpx>
```