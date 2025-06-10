using XingZhe.Model.User;
using XingZhe.Service;


namespace XingZhe.Model;

/// <summary>
/// 会话信息
/// </summary>
/// <param name="SessionId"></param>
/// <param name="UserInfo"></param>
/// <param name="Client"></param>
public record Session(string SessionId, UserInfo UserInfo, IXingZheClient Client);