using Common.Model.File.Fit;
using Dynastream.Fit;
using Microsoft.Extensions.Logging;
using UnitsNet;

namespace Common.Service.File;

public interface IFitService
{
    /// <summary>
    /// 从 fit 流反序列化
    /// </summary>
    /// <param name="fitSource"></param>
    Task<FitFile> DeserializeAsync(Stream fitSource);
}

public class FitService(ILogger<FitService> logger) : IFitService
{
    public async Task<FitFile> DeserializeAsync(Stream fitSource)
    {
        try
        {
            logger.LogTrace("正在读取Fit文件");

            // 复制 stream 内容
            using var memory = new MemoryStream();
            await fitSource.CopyToAsync(memory).ConfigureAwait(false);
            memory.Seek(0, SeekOrigin.Begin);

            var result =  await Task.Run(() => Create(memory));
            logger.LogTrace("Fit 文件读取完成");

            return result;
        }
        catch (FitException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new FitException("Fit文件加载失败", ex);
        }

        //创建 Fit 文件
        static FitFile Create(MemoryStream fitSource)
        {
            Decode decode = new();
            var fitFile = new FitFile();

            decode.MesgEvent += (_, e) =>
            {
                switch (e.mesg.Num)
                {
                    case MesgNum.FileId:
                        FileIdMesg fileIdMesg = new FileIdMesg(e.mesg);
                        fitFile.FileInfo = Mapper(fileIdMesg);
                        break;
                    case MesgNum.DeviceInfo:
                        DeviceInfoMesg deviceInfoMesg = new(e.mesg);
                        fitFile.DeviceInfo.Add(Mapper(deviceInfoMesg));
                        break;
                    case MesgNum.Record:
                        RecordMesg recordMesg = new(e.mesg);
                        fitFile.Records.Add(Mapper(recordMesg));
                        break;
                    case MesgNum.UserProfile:
                        UserProfileMesg userProfileMesg = new(e.mesg);
                        fitFile.UserProfiles.Add(Mapper(userProfileMesg));
                        break;
                }
            };

            decode.Read(fitSource);
            return fitFile;
        }
    }




    //映射文件信息
    private static Model.File.Fit.FileInfo Mapper(FileIdMesg mesg)
    {
        // 文件类型（如 Activity、Settings 等）
        var type = mesg.GetType();
        // 制造商编号
        var manufacturer = mesg.GetManufacturer();
        // 产品编号
        var product = mesg.GetProduct();
        // 设备序列号
        var serialNumber = mesg.GetSerialNumber();
        // 文件编号
        var number = mesg.GetNumber();
        // Garmin 产品编号
        var garminProduct = mesg.GetGarminProduct();
        // 创建时间（FIT SDK 中为自定义 DateTime 类型）
        var timeCreated = mesg.GetTimeCreated();


        return new Model.File.Fit.FileInfo
        {
            Type = type,
            Manufacturer = manufacturer,
            Product = product,
            SerialNumber = serialNumber,
            Number = number,
            GarminProduct = garminProduct,
            TimeCreated = timeCreated is null ? null : new DateTimeOffset(timeCreated.GetDateTime(), TimeSpan.Zero)
        };
    }

    //映射设备信息
    private static DeviceInfo Mapper(DeviceInfoMesg mesg)
    {
        // 设备索引（可能用于标识多个设备）
        var deviceIndex = mesg.GetDeviceIndex();
        // 制造商（Manufacturer 枚举，例如 Garmin、Wahoo 等）
        var manufacturer = mesg.GetManufacturer();
        // 产品 ID（产品的唯一编号）
        var product = mesg.GetProduct();
        // 设备序列号（用于唯一标识设备）
        var serialNumber = mesg.GetSerialNumber();
        // 软件版本（单位：版本号，例如 5.40）
        var softwareVersion = mesg.GetSoftwareVersion();
        // 硬件版本（单位：整数版本号）
        var hardwareVersion = mesg.GetHardwareVersion();
        // 电池电压（单位：伏特，范围 0~5）
        var batteryVoltage = mesg.GetBatteryVoltage();
        // 电池状态（BatteryStatus 枚举，例如 Good, Low, Critical）
        var batteryStatus = mesg.GetBatteryStatus();

        // 数据记录时间（从 1989-12-31 00:00:00 UTC 开始计时，单位：秒）
        var timestamp = mesg.GetTimestamp();

        // ANT+ 设备编号
        var antDeviceNumber = mesg.GetAntDeviceNumber();
        // ANT+ 传输类型（包含设备类型与传输方式信息）
        var antTransmissionType = mesg.GetAntTransmissionType();
        // ANT 网络类型（一般为 0 表示公共网络）
        var antNetwork = mesg.GetAntNetwork();
        // Garmin 产品编号（GarminProduct 枚举）
        var garminProduct = mesg.GetGarminProduct();


        return new DeviceInfo
        {
            DeviceIndex = deviceIndex,
            Manufacturer = manufacturer,
            Product = product,
            SerialNumber =  serialNumber,
            SoftwareVersion = softwareVersion,
            HardwareVersion =   hardwareVersion ,
            BatteryLevel = batteryVoltage is null ? null : Ratio.FromPercent(batteryVoltage.Value * 100f),
            BatteryStatus = batteryStatus       ,
            Timestamp = timestamp is null ? null : new DateTimeOffset(1989, 12, 31, 0, 0, 0, TimeSpan.Zero).AddSeconds(timestamp.GetTimeStamp()),
            AntDeviceNumber = antDeviceNumber,
            AntTransmissionType = antTransmissionType,
            AntNetwork = antNetwork,
            GarminProduct = garminProduct,
        };
    }

    //映射用户信息
    private static UserProfile Mapper(UserProfileMesg mesg)
    {
        // 用户自定义名称
        var friendlyName = mesg.GetFriendlyNameAsString();
        // 性别
        var gender = mesg.GetGender();
        // 年龄（岁）
        var age = mesg.GetAge();
        // 体重（千克）
        var weight = mesg.GetWeight();
        // 身高（米）
        var height = mesg.GetHeight();

        // 静息心率（次/分钟）
        var restingHeartRate = mesg.GetRestingHeartRate();
        // 最大跑步心率（次/分钟）
        var maxRunningHeartRate = mesg.GetDefaultMaxRunningHeartRate();
        // 最大骑行心率（次/分钟）
        var maxBikingHeartRate = mesg.GetDefaultMaxBikingHeartRate();
        // 最大心率（次/分钟）
        var mhr = mesg.GetDefaultMaxHeartRate();
        // 活动等级
        var activityClass = mesg.GetActivityClass();
        // 用户语言
        var language = mesg.GetLanguage();


        return new UserProfile
        {
            FriendlyName = friendlyName,
            Gender = gender,
            Age = age,

            Weight = weight is null ? null : global::UnitsNet.Mass.FromKilograms(weight.Value),
            Height = height is null ? null : global::UnitsNet.Length.FromMeters(height.Value),

            RestingHeartRate = restingHeartRate is null ? null : global::UnitsNet.Frequency.FromBeatsPerMinute(restingHeartRate.Value),
            MaxRunningHeartRate = maxRunningHeartRate is null ? null : global::UnitsNet.Frequency.FromBeatsPerMinute(maxRunningHeartRate.Value),
            MaxBikingHeartRate = maxBikingHeartRate is null ? null : global::UnitsNet.Frequency.FromBeatsPerMinute(maxBikingHeartRate.Value),
            MHR = mhr is null ? null : global::UnitsNet.Frequency.FromBeatsPerMinute(mhr.Value),

            ActivityClass = activityClass,
            Language = language
        };
    }

    //映射采样点
    private static Record Mapper(RecordMesg mesg)
    {
        //时间 (秒，起始于 UTC 时间 1989-12-31 00:00:00)
        var timestamp = (uint?)mesg.GetField(RecordMesg.FieldDefNum.Timestamp)?.GetValue();
        //维度 (经纬度在 FIT 文件中使用 semicircle 表示法，范围是 ±2^31，对应 ±180°)
        var lon = (int?)mesg.GetField(RecordMesg.FieldDefNum.PositionLong)?.GetValue();
        //经度 (经纬度在 FIT 文件中使用 semicircle 表示法，范围是 ±2^31，对应 ±180°)
        var lat = (int?)mesg.GetField(RecordMesg.FieldDefNum.PositionLat)?.GetValue();
        //海拔 (米)
        var altitude = (float?)mesg.GetField(RecordMesg.FieldDefNum.EnhancedAltitude)?.GetValue();
        altitude ??= (float?)mesg.GetField(RecordMesg.FieldDefNum.Altitude)?.GetValue();

        //心率 (次/分)
        var heartRate = (byte?)mesg.GetField(RecordMesg.FieldDefNum.HeartRate)?.GetValue();
        //踏频 (圈/分)
        var cadence = (byte?)mesg.GetField(RecordMesg.FieldDefNum.Cadence)?.GetValue();
        //速度 (米/秒)
        var speed = (float?)mesg.GetField(RecordMesg.FieldDefNum.Speed)?.GetValue();
        //距离 (米)
        var distance = (float?)mesg.GetField(RecordMesg.FieldDefNum.Distance)?.GetValue();
        //功率 (瓦)
        var power = (ushort?)mesg.GetField(RecordMesg.FieldDefNum.Power)?.GetValue();
        //温度 (摄氏度)
        var temperature = (sbyte?)mesg.GetField(RecordMesg.FieldDefNum.Temperature)?.GetValue();


        return new Record()
        {
            Timestamp = timestamp is null ? null : new DateTimeOffset(1989, 12, 31, 0, 0, 0, TimeSpan.Zero).AddSeconds(timestamp.Value),
            Longitude = lon is null ? null : lon * (180.0 / Math.Pow(2, 31)),
            Latitude = lat is null ? null : lat * (180.0 / Math.Pow(2, 31)),

            Heartrate = heartRate is null ? null : Frequency.FromBeatsPerMinute(heartRate.Value),
            Cadence = cadence is null ? null : Frequency.FromCyclesPerMinute(cadence.Value),
            Speed = speed is null ? null : Speed.FromMetersPerSecond(speed.Value),
            Distance = distance is null ? null : Length.FromMeters(distance.Value),
            Altitude = altitude is null ? null : Length.FromMeters(altitude.Value),
            Power = power is null ? null : Power.FromWatts(power.Value),
            Temperature = temperature is null ? null : Temperature.FromDegreesCelsius(temperature.Value)
        };
    }
}
