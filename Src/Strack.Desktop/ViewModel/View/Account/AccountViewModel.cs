using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IGPSport.Service;
using Strack.Desktop.Extension;
using Strack.Desktop.UI.View.Account.Add;
using Strack.Desktop.ViewModel.View.Account.Add;
using Strack.Desktop.ViewModel.View.Account.Card;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;
using XingZhe.Service;

namespace Strack.Desktop.ViewModel.View.Account;

/// <summary>
/// 卡片视图
/// </summary>
public partial class AccountViewModel(
    ISnackbarService snackbarService,
    IXingZheClientProvider xingZheClientProvider,
    IContentDialogService contentDialogService,
    IXingZheSetting xingZheSetting,
    IIGPSportSetting iGPSportSetting
    ) : ObservableObject
{
    /// <summary>
    /// 所有账户
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<CardViewModel> ItemsSource { get; set; } = [];


    /// <summary>
    /// 添加账户
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task AddFromXingZheSessionAsync()
    {
        XingZheSessionViewModel vm = new(xingZheClientProvider);
        var result = await contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
        {
            Title = "添加行者会话",
            Content = new XingZheSessionView() { DataContext = vm },
            PrimaryButtonText = "添加",
            CloseButtonText = "关闭"
        });

        if (result != ContentDialogResult.Primary) return;

        if (string.IsNullOrWhiteSpace(vm.SessionId) || vm.UserInfo == null || vm.Client == null)
        {
            snackbarService.ShowError("会话信息添加失败, 未与服务器获得响应");
            return;
        }

        var card = new CardViewModel()
        {
            Id = vm.UserInfo.Id,
            Name = vm.UserInfo.Name,
            AvatarSource = new BitmapImage(new Uri(vm.UserInfo.AvatarUrl))
        };
        card.DeleteCommand = new RelayCommand(() =>
        {
            ItemsSource.Remove(card);
        });
        card.RefreshCommand = new AsyncRelayCommand(async () =>
        {
            try
            {
                var user = await vm.Client.GetUserInfoAsync();
                card.AvatarSource = new BitmapImage(new Uri(user.AvatarUrl));
                card.Id = user.Id;
                card.Name = user.Name;
            }
            catch(Exception ex)
            {
                snackbarService.ShowError(ex.Message, "刷新失败");
            }
        });

        ItemsSource.Add(card);
    }


    [RelayCommand]
    private async Task ReloadAsync()
    {
        foreach (var sessionId in xingZheSetting.SessionIds)
        {
            try
            {
                var client = xingZheClientProvider.GetOrCreateFromSessionId(sessionId);
                var userInfo = await client.GetUserInfoAsync();

                ItemsSource.Add(new CardViewModel()
                {
                    AvatarSource = new BitmapImage(new Uri(userInfo.AvatarUrl)),
                    Name = userInfo.Name,
                    Id = userInfo.Id
                });

                //
                await Task.Delay(10);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
