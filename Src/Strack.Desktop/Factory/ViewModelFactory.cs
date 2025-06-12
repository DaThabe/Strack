using CommunityToolkit.Mvvm.Input;
using Strack.Desktop.Extension;
using Strack.Desktop.ViewModel.View.Account.Card;
using System.Windows.Media.Imaging;
using XingZhe.Model.User;

namespace Strack.Desktop.Factory;

internal static class ViewModelFactory
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="services"></param>
    /// <param name="init"></param>
    /// <param name="remove"></param>
    /// <param name="getUserInfoAsync"></param>
    public static async Task<CardViewModel> CreateAccountCardViewModelFromXingZheAsync(this IServiceProvider services, Action<CardViewModel> remove, Func<Task<UserInfo>> getUserInfoAsync)
    {
        var card = new CardViewModel();
        card.DeleteCommand = new RelayCommand(() =>
        {
            remove(card);
        });
        card.RefreshCommand = new AsyncRelayCommand(async () =>
        {
            try
            {
                var user = await getUserInfoAsync();
                card.AvatarSource = new BitmapImage(new Uri(user.AvatarUrl));
                card.Id = user.Id;
                card.Name = user.Name;
                card.IsVerified = true;
            }
            catch (Exception ex)
            {
                card.IsVerified = false;
                services.GetISnackbarService().ShowError(ex.Message, "刷新失败");
            }
        });

        await card.RefreshCommand.ExecuteAsync(null);
        return card;
    }
}
