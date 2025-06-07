using Dynastream.Fit;
using UnitsNet;

namespace Common.Model.File.Fit;


/// <summary>
/// 设备信息
/// </summary>
public class DeviceInfo
{
    /// <summary>
    /// 设备唯一标识
    /// </summary>
    public uint? DeviceIndex { get; set; }

    /// <summary>
    /// 制造商
    /// </summary>
    public ushort? Manufacturer { get; set; }

    /// <summary>
    /// 产品编号
    /// </summary>
    public ushort? Product { get; set; }

    /// <summary>
    /// 序列号
    /// </summary>
    public ulong? SerialNumber { get; set; }

    /// <summary>
    /// 软件版本
    /// </summary>
    public float? SoftwareVersion { get; set; }

    /// <summary>
    /// 硬件版本
    /// </summary>
    public byte? HardwareVersion { get; set; }

    /// <summary>
    /// 电池电量（百分比）
    /// </summary>
    public Ratio? BatteryLevel { get; set; }

    /// <summary>
    /// 电池状态（如正常、低电）
    /// </summary>
    public byte? BatteryStatus { get; set; }

    /// <summary>
    /// 设备时间（UTC）
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// ANT ID 设备号
    /// </summary>
    public ushort? AntDeviceNumber { get; set; }

    /// <summary>
    /// ANT 通道类型
    /// </summary>
    public byte? AntTransmissionType { get; set; }

    /// <summary>
    /// ANT 网络编号
    /// </summary>
    public AntNetwork? AntNetwork { get; set; }

    /// <summary>
    /// Garmin 产品枚举
    /// </summary>
    public ushort? GarminProduct { get; set; }
}