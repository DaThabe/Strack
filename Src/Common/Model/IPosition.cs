namespace Common.Model;


/// <summary>
/// 位置接口 (经纬度)
/// </summary>
public interface IPosition
{
    /// <summary>
    /// 经度
    /// </summary>
     double Longitude { get; set; }

    /// <summary>
    /// 纬度
    /// </summary>
     double Latitude { get; set; }
}
