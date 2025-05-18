using System.Numerics;

namespace Strack.Extension;

internal static class CalcExtension
{
    /// <summary>
    /// 计算一组时间内的总间隔
    /// </summary>
    /// <param name="timestamps"></param>
    /// <returns></returns>
    public static TimeSpan CalcTotalSpan(this IEnumerable<DateTime> timestamps)
    {
        var sorted = timestamps.OrderBy(t => t).ToArray();
        if (sorted.Length < 2)
            return TimeSpan.Zero;

        TimeSpan total = TimeSpan.Zero;
        for (int i = 1; i < sorted.Length; i++)
        {
            total += sorted[i] - sorted[i - 1];
        }

        return total;
    }


    /// <summary>
    /// 计算序列的累计上升和下降总量（只考虑正向变化）
    /// </summary>
    public static (double TotalGain, double TotalLoss) CalcCumulativeGainLoss(this IEnumerable<double> values)
    {
        double totalGain = 0, totalLoss = 0;
        var arr = values.ToArray();

        for (int i = 1; i < arr.Length; i++)
        {
            double diff = arr[i] - arr[i - 1];

            if (diff > 0)
                totalGain += diff;
            else if (diff < 0)
                totalLoss -= diff; // 取正值
        }

        return (totalGain, totalLoss);
    }

    /// <summary>
    /// 计算最大值、最小值、平均值和总和
    /// </summary>
    public static (T Min, T Max, T Avg, T Sum) CalcMinMaxAvgSum<T>(this IEnumerable<T> values) where T : INumber<T>
    {
        var arr = values.ToArray();

        if (arr.Length == 0)
            throw new InvalidOperationException("序列不能为空");

        T min = arr[0];
        T max = arr[0];
        T sum = T.Zero;

        foreach (var v in arr)
        {
            if (v < min) min = v;
            if (v > max) max = v;
            sum += v;
        }

        T avg = sum / T.CreateChecked(arr.Length);

        return (min, max, avg, sum);
    }
}