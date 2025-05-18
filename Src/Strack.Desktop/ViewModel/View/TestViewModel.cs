using Microsoft.Extensions.Logging;
using Strack.Desktop.ViewModel.Shell.Navigation.Page;

namespace Strack.Desktop.ViewModel.View;

public class TestViewModel(ILogger<TestViewModel> logger) : INavigationPageViewModel
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
