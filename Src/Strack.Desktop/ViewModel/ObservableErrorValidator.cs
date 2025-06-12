using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Strack.Desktop.ViewModel;

public partial class ObservableErrorValidator : ObservableObject, INotifyDataErrorInfo
{
    /// <summary>
    /// 是否有错误
    /// </summary>
    [ObservableProperty]
    public partial bool HasErrors { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    [ObservableProperty]
    public partial string ErrorString { get; set; }

    /// <summary>
    /// 错误通知
    /// </summary>
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


    /// <summary>
    /// 获取错误
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName)) return Array.Empty<ValidationResult>();

        _errors.TryGetValue(propertyName, out var errors);
        return errors ?? [];
    }

    /// <summary>
    /// 设置验证结果
    /// </summary>
    /// <param name="result"></param>
    /// <param name="propertyName"></param>
    public void AddError(string message, string propertyName)
    {
        if (!_errors.TryGetValue(propertyName, out var results))
        {
            results = [];
            _errors[propertyName] = results;
        }

        _errors[propertyName].Add(new ValidationResult(message));
        OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
    }

    /// <summary>
    /// 清除错误
    /// </summary>
    /// <param name="propertyName"></param>
    public void ClearError(string propertyName)
    {
        if (!_errors.TryGetValue(propertyName, out var results))
        {
            return;
        }

        if (_errors.Remove(propertyName)) OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
    }

    /// <summary>
    /// 清除所有错误
    /// </summary>
    public void ClearAllError()
    {
        var names = _errors.Keys.ToArray();
        _errors.Clear();

        foreach (var propertyName in names) OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
    }

    /// <summary>
    /// 通知该属性的错误
    /// </summary>
    /// <param name="propertyName"></param>
    public void OnErrorsChanged(DataErrorsChangedEventArgs args)
    {
        ErrorsChanged?.Invoke(this, args);

        HasErrors = _errors.Count > 0;
        ErrorString = string.Join('\n', from i in _errors.Values from j in i select j.ErrorMessage);
    }

    /// <summary>
    /// 给该属性设置一个错误并通知
    /// </summary>
    /// <param name="message"></param>
    /// <param name="propertyName"></param>
    public void SetError(string message, string propertyName)
    {
        if (!_errors.TryGetValue(propertyName, out var results))
        {
            results = [];
            _errors[propertyName] = results;
        }

        _errors[propertyName] = [new ValidationResult(message)];
        OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
    }


    // 所有
    private readonly Dictionary<string, List<ValidationResult>> _errors = [];
}