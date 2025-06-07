namespace Common.Model.File.Fit;


/// <summary>
/// 文件信息
/// </summary>
public class FileInfo
{
    /// <summary>
    /// 文件类型（如 Activity、Device 等）
    /// </summary>
    public Dynastream.Fit.File? Type { get; set; }

    /// <summary>
    /// 制造商编号
    /// </summary>
    public ushort? Manufacturer { get; set; }

    /// <summary>
    /// 产品型号 ID
    /// </summary>
    public ushort? Product { get; set; }

    /// <summary>
    /// 设备序列号
    /// </summary>
    public uint? SerialNumber { get; set; }

    /// <summary>
    /// 创建时间（UTC）
    /// </summary>
    public DateTimeOffset? TimeCreated { get; set; }

    /// <summary>
    /// 文件编号
    /// </summary>
    public ushort? Number { get; set; }

    /// <summary>
    /// Garmin 软件版本
    /// </summary>
    public ushort? GarminProduct { get; set; }
}