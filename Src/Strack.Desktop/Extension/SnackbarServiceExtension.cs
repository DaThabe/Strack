using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Strack.Desktop.Extension;

public static class SnackbarServiceExtension
{
    public static IconElement DefaultPrimaryIcon { get; } = new SymbolIcon(SymbolRegular.ChatSparkle24);
    public static string DefaultPrimaryTitle { get; } = "重要";
    public static double DefaultPrimaryDelaySeconds { get; } = 3.5;

    public static IconElement DefaultErrorIcon { get; } = new SymbolIcon(SymbolRegular.DismissCircle24);
    public static string DefaultErrorTitle { get; } = "错误";
    public static double DefaultErrorDelaySeconds { get; } = 3;


    public static IconElement DefaultWarningIcon { get; } = new SymbolIcon(SymbolRegular.Warning24);
    public static string DefaultWarningTitle { get; } = "警告";
    public static double DefaultWarningDelaySeconds { get; } = 2;

    public static IconElement DefaultSuccessIcon { get; } = new SymbolIcon(SymbolRegular.Checkmark24);
    public static string DefaultSuccessTitle { get; } = "成功";
    public static double DefaultSuccessDelaySeconds { get; } = 1.5;


    public static IconElement DefaultInfoIcon { get; } = new SymbolIcon(SymbolRegular.AlertUrgent24);
    public static string DefaultInfoTitle { get; } = "提示";
    public static double DefaultInfoDelaySeconds { get; } = 1;


    public static void ShowError(this ISnackbarService service, string message, string? title = null, IconElement? icon = null, double? delaySeconds = null)
    {
        service.Show(
            title ?? DefaultErrorTitle,
            message, ControlAppearance.Danger,
            icon ?? DefaultErrorIcon,
            TimeSpan.FromSeconds(delaySeconds ?? DefaultErrorDelaySeconds));
    }
    public static void ShowWarning(this ISnackbarService service, string message, string? title = null, IconElement? icon = null, double? delaySeconds = null)
    {
        service.Show(
            title ?? DefaultWarningTitle,
            message, ControlAppearance.Caution,
            icon ?? DefaultWarningIcon,
            TimeSpan.FromSeconds(delaySeconds ?? DefaultWarningDelaySeconds));
    }
    public static void ShowSuccess(this ISnackbarService service, string message, string? title = null, IconElement? icon = null, double? delaySeconds = null)
    {
        service.Show(
            title ?? DefaultSuccessTitle,
            message, ControlAppearance.Success,
            icon ?? DefaultSuccessIcon,
            TimeSpan.FromSeconds(delaySeconds ?? DefaultSuccessDelaySeconds));
    }
    public static void ShowInfo(this ISnackbarService service, string message, string? title = null, IconElement? icon = null, double? delaySeconds = null)
    {
        service.Show(
            title ?? DefaultInfoTitle,
            message, ControlAppearance.Info,
            icon ?? DefaultInfoIcon,
            TimeSpan.FromSeconds(delaySeconds ?? DefaultInfoDelaySeconds));
    }
    public static void ShowPrimary(this ISnackbarService service, string message, string? title = null, IconElement? icon = null, double? delaySeconds = null)
    {
        service.Show(
            title ?? DefaultPrimaryTitle,
            message, ControlAppearance.Primary,
            icon ?? DefaultPrimaryIcon,
            TimeSpan.FromSeconds(delaySeconds ?? DefaultPrimaryDelaySeconds));
    }
}