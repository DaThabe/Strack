using UnitsNet;

namespace Common.Model.Data;

/// <summary>
/// 地理位置 (经纬度,海拔)
/// </summary>
public interface IGeoPosition : IPosition
{
    Length? Altitude { get; set; }
}