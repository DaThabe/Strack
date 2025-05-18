namespace Strack.Desktop.ViewModel.Shell.Navigation.Page;

public interface INavigationPageViewModel
{
    /// <summary>
    /// 导航进入
    /// </summary>
    /// <returns></returns>
    Task NavigationToAsync();

    /// <summary>
    /// 导航离开
    /// </summary>
    /// <returns></returns>
    Task NavigationFromAsync();
}
