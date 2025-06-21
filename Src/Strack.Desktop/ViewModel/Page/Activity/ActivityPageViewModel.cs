using Common.Infrastructure;
using CommunityToolkit.Mvvm.Input;
using FluentFrame.Service.Shell.Dialog;
using FluentFrame.Service.Shell.Message;
using FluentFrame.Service.Shell.Notify;
using FluentFrame.ViewModel.Shell.Message;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Strack.Data.Extension;
using Strack.Desktop.Extension;
using Strack.Desktop.UI.Page.Activity.User;
using Strack.Desktop.ViewModel.Page.Activity.Pagination;
using Strack.Desktop.ViewModel.View.Activity.Summary;
using Strack.Desktop.ViewModel.View.Activity.User;
using Strack.Model.Entity.Enum;
using Strack.Service;
using Strack.Service.Activity;
using System.Collections.ObjectModel;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using UnitsNet;
using Wpf.Ui.Abstractions.Controls;

namespace Strack.Desktop.ViewModel.View.Activity;


/// <summary>
/// 活动信息
/// </summary>
public partial class ActivityPageViewModel(
    IServiceProvider services,
    IStrackDbService dbService,
    IDialogService dialogService
    ) : ObservableObject, INavigationAware
{
    #region --属性--

    /// <summary>
    /// 当前选中用户
    /// </summary>
    [ObservableProperty]
    public partial UserViewModel? SelectedUser { get; set; }

    /// <summary>
    /// 用户列表
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<UserViewModel> Users { get; set; } = [];

    #endregion

    #region --行为--

    //选择用户
    partial void OnSelectedUserChanged(UserViewModel? value)
    {
        value?.ReloadActivityCommand.Execute(null);
    }

    //导航进入
    public async Task OnNavigatedToAsync()
    {
        await ReloadUserCommand.ExecuteAsync(null);
        if (SelectedUser != null) await SelectedUser.OnNavigatedToAsync();
    }

    //导航离开
    public async Task OnNavigatedFromAsync()
    {
        if (SelectedUser != null) await SelectedUser.OnNavigatedFromAsync();
        await _reloadUserCancellableOperation.CancelAsync();
    }

    #endregion

    #region --命令--

    /// <summary>
    /// 加载用户
    /// </summary>
    [RelayCommand]
    private async Task ReloadUserAsync()
    {
        var cancellationToken = await _reloadUserCancellableOperation.StartNew();

        var userEntities = await dbService.ExecuteAsync(x => x.GetAllUserAsync(cancellationToken), cancellationToken);

        Users.Clear();
       
        foreach (var user in userEntities)
        {
            var userVm = services.GetRequiredService<UserViewModel>();
            userVm.EntityId = user.Id;
            userVm.PlatformType = user.Platform;
            userVm.Name = user.Name ?? $"{user.Platform}{user.Id}";
            userVm.Avatar = user.AvatarUrl is null ? null : new BitmapImage(new Uri(user.AvatarUrl));

            Users.Add(userVm);
            await Task.Delay(10, cancellationToken); //防止UI卡顿
        }

        SelectedUser = Users.FirstOrDefault();
    }

    /// <summary>
    /// 添加用户
    /// </summary>
    [RelayCommand]
    private async Task AddUserAsync()
    {
        var userAdderView = services.GetRequiredService<UserAdderView>();

        await dialogService.ShowInfoDialogAsync(userAdderView, "添加用户");
        await ReloadUserCommand.ExecuteAsync(null);
    }

    #endregion

    private readonly CancellableOperation _reloadUserCancellableOperation = new();
}