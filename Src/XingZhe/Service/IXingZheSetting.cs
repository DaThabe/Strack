using Common.Service.Setting;

namespace XingZhe.Service;


/// <summary>
/// 配置服务
/// </summary>
public interface IXingZheSetting
{
    /// <summary>
    /// 行者会话Id
    /// </summary>
    string[] SessionIds { get; set; }
}


public class XingZheSetting(ISettingService setting) : IXingZheSetting
{
    public string[] SessionIds
    {
        get => setting.Get<string[]>("XingZheSessionIds") ?? [];
        set => _ = setting.SetAndSaveAsync("XingZheSessionIds", value);
    }
}