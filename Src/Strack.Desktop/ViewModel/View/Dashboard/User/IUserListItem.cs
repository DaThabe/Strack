using CommunityToolkit.Mvvm.Input;
using Strack.Model.Entity.Enum;
using System.Windows.Media.Imaging;

namespace Strack.Desktop.ViewModel.View.Dashboard.User;


/// <summary>
/// 用户列表元素
/// </summary>
public interface IUserListItem
{
    /// <summary>
    /// 名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    PlatformType Source { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    BitmapImage Avatar { get; set; }

    /// <summary>
    /// 点击更多命令
    /// </summary>
    IRelayCommand ClickMoreCommand { get; }
}
