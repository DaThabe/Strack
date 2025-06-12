using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Strack.Desktop.Service;
using XingZhe.Model.User;
using XingZhe.Service;

namespace Strack.Desktop.ViewModel.View.Account.Add;


/// <summary>
/// 行者会话
/// </summary>
public partial class XingZheSessionViewModel(IXingZheClientProvider clientProvider) : ObservableErrorValidator
{
    /// <summary>
    /// 行者会话Id
    /// </summary>
    [ObservableProperty]
    public partial string? SessionId { get; private set; }

    /// <summary>
    /// 当前用户
    /// </summary>
    [ObservableProperty]
    public partial UserInfo? UserInfo { get; private set; }

    /// <summary>
    /// 请求客户端
    /// </summary>
    public IXingZheClient? Client { get; private set; }


    [RelayCommand]
    private async Task VerifySessionId()
    {
        Client = null;
        UserInfo = null;
        ClearAllError();

        if(string.IsNullOrWhiteSpace(SessionId))
        {
            SetError("会话Id不可为空", nameof(SessionId));
            return;
        }

        try
        {
            Client = clientProvider.GetOrCreateFromSessionId(SessionId);
            UserInfo = await Client.GetUserInfoAsync() ?? throw new ArgumentException("获取用户信息失败,请检查SessionId是否有效");
        }
        catch(Exception ex)
        {
            SetError(ex.Message, nameof(UserInfo));
        }

        //避免用户疯狂点击
        await Task.Delay(1000);
    }
}