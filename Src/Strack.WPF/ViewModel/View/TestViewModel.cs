using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Strack.Desktop.ViewModel.Shell.Navigation.Page;

namespace Strack.Desktop.ViewModel.View;

public class TestViewModel(ILogger<TestViewModel> logger) : ObservableObject, INavigationPageViewModel
{
    public Task NavigationFromAsync()
    {
        logger.LogInformation("离开");
        return Task.CompletedTask;
    }

    public Task NavigationToAsync()
    {
        logger.LogInformation("进入");
        return Task.CompletedTask;
    }
}
