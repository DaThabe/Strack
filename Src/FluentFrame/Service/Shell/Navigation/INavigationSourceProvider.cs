namespace FluentFrame.Service.Shell.Navigation;

public interface INavigationSourceProvider
{
    /// <summary>
    /// 页面内容
    /// </summary>
    object? Content { get; set; }
}