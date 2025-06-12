using Common.Service.Setting;

namespace IGPSport.Service;


/// <summary>
/// 设置服务
/// </summary>
public interface IIGPSportSetting
{
    /// <summary>
    /// 授权码
    /// </summary>
    string[] AuthTokens { get; set; }
}


public class IGPSportSetting(ISettingService setting) : IIGPSportSetting
{
    public string[] AuthTokens
    {
        get => setting.Get<string[]>("IIGPSportAuthTokens") ?? [];
        set => _ = setting.SetAndSaveAsync("IIGPSportAuthTokens", value);
    }
}