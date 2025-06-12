using Strack.Desktop.ViewModel.View.Account;
using Wpf.Ui.Abstractions.Controls;

namespace Strack.Desktop.UI.View.Account;

public partial class AccountView : INavigableView<AccountViewModel>
{
    public AccountView(AccountViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    public AccountViewModel ViewModel => (AccountViewModel)DataContext;
}
