using FluentFrame.ViewModel.Shell.Message;
using System.Collections.ObjectModel;

namespace FluentFrame.Service.Shell.Message;

public interface IMessageSourceProvider
{
    ObservableCollection<MessageItemViewModel> ItemsSource { get; }
}