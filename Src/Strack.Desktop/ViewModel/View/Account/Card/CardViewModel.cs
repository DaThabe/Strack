using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Media;

namespace Strack.Desktop.ViewModel.View.Account.Card;


/// <summary>
/// 卡片图模型
/// </summary>
public partial class CardViewModel : ObservableObject
{
    /// <summary>
    /// 头像
    /// </summary>
    [ObservableProperty] public partial ImageSource? AvatarSource { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [ObservableProperty] public partial string? Name { get; set; }

    /// <summary>
    /// Id
    /// </summary>
    [ObservableProperty] public partial long? Id { get; set; }

    /// <summary>
    /// 是否验证
    /// </summary>
    [ObservableProperty] public partial bool IsVerified { get; set; }

    /// <summary>
    /// 刷新命令
    /// </summary>
    public IAsyncRelayCommand? RefreshCommand { get; set; }

    /// <summary>
    /// 删除命令
    /// </summary>
    public IRelayCommand? DeleteCommand { get; set; }
}