using FluentFrame.ViewModel.Shell.Notify;
using System.Collections.ObjectModel;

namespace FluentFrame.Service.Shell.Notify;

public interface INotifySourceProvider
{
    ObservableCollection<NotifyItemViewModel> ItemsSource { get; }
}