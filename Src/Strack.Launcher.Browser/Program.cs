using System.Threading.Tasks;

using Avalonia;
using Avalonia.Browser;
using Strack.UI;

internal sealed class Program
{
    private static Task Main(string[] args) => AppBuilder
            .Configure<App>()
            .WithInterFont()
            .StartBrowserAppAsync("out");
}
