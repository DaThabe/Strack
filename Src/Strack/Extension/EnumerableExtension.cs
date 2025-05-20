using System.Collections.ObjectModel;

namespace Strack.Extension;

public static class EnumerableExtension
{
    /// <summary>
    /// 转为ObservableCollection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> values)
    {
        if(values is ObservableCollection<T> collection)
        {
            return collection;
        }

        return new ObservableCollection<T>(values);
    }
}
