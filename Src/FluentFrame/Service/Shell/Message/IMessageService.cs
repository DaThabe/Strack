using FluentFrame.ViewModel.Shell.Message;

namespace FluentFrame.Service.Shell.Message;

/// <summary>
/// 消息服务
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// 设置资源委托
    /// </summary>
    /// <param name="sourceProvider"></param>
    public void SetSourceProvider(IMessageSourceProvider sourceProvider);


    /// <summary>
    /// 添加消息
    /// </summary>
    /// <param name="item"></param>
    void Add(MessageItemViewModel item);

    /// <summary>
    /// 删除消息
    /// </summary>
    /// <param name="item"></param>
    void Remove(MessageItemViewModel item);
}


public class MessageService : IMessageService
{
    public void SetSourceProvider(IMessageSourceProvider sourceProvider) => _sourceProvider = sourceProvider;

    public void Add(MessageItemViewModel item)
    {
        if (!_sourceProvider.ItemsSource.Contains(item))
        {
           _sourceProvider.ItemsSource.Add(item);
        }
    }
    public void Remove(MessageItemViewModel item)
    {
        if (_sourceProvider.ItemsSource.Contains(item))
        {
            _sourceProvider.ItemsSource.Remove(item);
        }
    }


    private IMessageSourceProvider _sourceProvider = null!;
}