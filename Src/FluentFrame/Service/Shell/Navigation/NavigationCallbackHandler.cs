namespace FluentFrame.Service.Shell.Navigation;

/// <summary>
/// 导航回调
/// </summary>
/// <param name="to">目标</param>
/// <param name="from">来源</param>
public delegate Task<bool> NavigationCallbackHandler(object to, object? from);