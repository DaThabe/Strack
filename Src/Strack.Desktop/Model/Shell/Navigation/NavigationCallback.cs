using Strack.Desktop.ViewModel.Shell.Navigation;

namespace Strack.Desktop.Model.Shell.Navigation;


/// <summary>
/// 导航回调
/// </summary>
/// <param name="to">目标</param>
/// <param name="from">来源</param>
/// <returns></returns>
public delegate Task<bool> NavigationCallback(NavigationItemViewModel to, NavigationItemViewModel? from);
