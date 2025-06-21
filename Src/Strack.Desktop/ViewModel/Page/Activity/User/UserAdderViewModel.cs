using CommunityToolkit.Mvvm.Input;
using FluentFrame.Service.Shell.Notify;
using Strack.Model.Entity.Enum;
using Strack.Service.User;
using System.Collections.ObjectModel;

namespace Strack.Desktop.ViewModel.Page.Activity.User;

public partial class UserAdderViewModel(
    INotifyService notifyService,
    IUserCredentialService userCredentialService
    ) : ObservableObject
{
    /// <summary>
    /// 平台类型
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<PlatformType> Platforms { get; set; } =
    [
        PlatformType.XingZhe,
        PlatformType.IGPSport,
    ];

    /// <summary>
    /// 凭证类型
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<CredentialType> Credentials { get; set; } = [];


    /// <summary>
    /// 选中的平台类型
    /// </summary>
    [ObservableProperty]
    public partial PlatformType? SelectedPlatform { get; set; }

    /// <summary>
    /// 选中的凭证类型
    /// </summary>
    [ObservableProperty] 
    public partial CredentialType? SelectedCredential { get; set; }

    /// <summary>
    /// 凭证内容
    /// </summary>
    [ObservableProperty] 
    public partial string? Content { get; set; }


    /// <summary>
    /// 添加用户凭证
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task AddAsync()
    {
        if(string.IsNullOrWhiteSpace(Content))
        {
            await notifyService.ShowErrorAsync("凭证内容不可为空");
            return;
        }

        if (SelectedPlatform == null)
        {
            await notifyService.ShowErrorAsync("平台类型不可为空");
            return;
        }

        if (SelectedCredential == null)
        {
            await notifyService.ShowErrorAsync("凭证类型不可为空");
            return;
        }

        try
        {

            await userCredentialService.UpsertAsync(SelectedPlatform.Value, SelectedCredential.Value, Content);
            await notifyService.ShowSuccessAsync("用户凭证添加成功");
        }
        catch(Exception ex)
        {
            await notifyService.ShowErrorAsync(ex.Message, "用户凭证添加失败");
        }
    }


    partial void OnSelectedPlatformChanged(PlatformType? value)
    {
        if (value == null) return;

        if(value == PlatformType.XingZhe)
        {
            Credentials = [CredentialType.SessionId];
            SelectedCredential = Credentials[0];
            return;
        }

        if(value == PlatformType.IGPSport)
        {
            Credentials = [CredentialType.AuthToken];
            SelectedCredential = Credentials[0];
            return;
        }
    }
}