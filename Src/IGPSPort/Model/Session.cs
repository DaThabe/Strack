using IGPSport.Model.User;
using IGPSport.Service;
using System.Net;


namespace IGPSport.Model;

/// <summary>
/// 会话信息
/// </summary>
/// <param name="Authorization"></param>
/// <param name="UserInfo"></param>
/// <param name="Client"></param>
public record Session(string Authorization, UserInfo UserInfo, IIGPSportClient Client);