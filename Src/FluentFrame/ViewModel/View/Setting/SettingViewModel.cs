using CommunityToolkit.Mvvm.Input;
using FluentFrame.UI.Shell;
using FluentFrame.ViewModel.View.Setting.Category;
using System.Collections.ObjectModel;
using System.Windows;
using Wpf.Ui.Controls;

namespace FluentFrame.ViewModel.View.Setting;


/// <summary>
/// 设置页
/// </summary>
public partial class SettingViewModel : ObservableObject
{
    /// <summary>
    /// 所有类别
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<CategoryViewModel> CategoryItems { get; set; } = [];


    [RelayCommand]
    private void OnClick()
    {
        var shell = Application.Current.MainWindow as FluentShell;

        shell?.ViewModel?.Notifies.Add(new() { Appearance = GetRandomEnumValue(), Title = $"成功", Delay = TimeSpan.FromSeconds(Random.Shared.Next(3)) });

    }

    public static ControlAppearance GetRandomEnumValue()
    {
        var values = Enum.GetValues<ControlAppearance>();
        Random random = new();
        int index = random.Next(values.Length);
        return (ControlAppearance)values.GetValue(index)!;
    }
}
