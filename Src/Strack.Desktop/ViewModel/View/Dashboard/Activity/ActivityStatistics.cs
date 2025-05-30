﻿using CommunityToolkit.Mvvm.ComponentModel;
using Strack.Model;
using UnitsNet;

namespace Strack.Desktop.ViewModel.View.Dashboard.Activity;


/// <summary>
/// 活动数据统计
/// </summary>
public partial class ActivityStatistics : ObservableObject
{
    /// <summary>
    /// 序号
    /// </summary>
    [ObservableProperty]
    public partial int SerialNumber { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TypeFormatted))]
    public partial ActivityType Type { get; set; }

    /// <summary>
    /// 总距离
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DistanceKilometerFormatted))]
    public partial Length Distance { get; set; }

    /// <summary>
    /// 持续时间
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DurationFormatted))]
    public partial TimeSpan Duration { get; set; }


    /// <summary>
    /// 类型格式化
    /// </summary>
    public string TypeFormatted
    {
        get => Type.ToName();
    }

    /// <summary>
    /// 时间格式化
    /// </summary>
    public string DurationFormatted
    {
        get
        {
            //小于1分钟 展示秒
            if (Duration.TotalMinutes <= 1)
            {
                return $"{Duration.TotalSeconds}秒";
            }

            //小于1小时 展示分秒
            if (Duration.TotalHours <= 1)
            {
                return $"{Duration.Minutes}分{Duration.Seconds}秒";
            }

            //展示时分
            return $"{(int)Duration.TotalHours}时{Duration.Minutes}分";
        }
    }

    /// <summary>
    /// 活动距离格式化
    /// </summary>
    public string DistanceKilometerFormatted
    {
        get
        {
            double km = Distance.Kilometers;

            if (km < 1)
            {
                return $"{Distance.Meters:F0}米";
            }

            if (km < 1_000)
            {
                return $"{km:F2}千米";
            }

            if (km < 1_000_000)
            {
                double value = km / 10_000.0;
                if (value < 1000)
                    return $"{value:F2}万公里";
            }

            if (km < 100_000_000)
            {
                double value = km / 1_000_000.0;
                if (value < 1000)
                    return $"{value:F2}百万公里";
            }

            if (km < 1_000_000_000)
            {
                double value = km / 100_000_000.0;
                if (value < 1000)
                    return $"{value:F2}亿公里";
            }

            if (km < 1e12)
            {
                double value = km / 1_000_000_000.0;
                if (value < 1000)
                    return $"{value:F2}十亿公里";
            }

            if (km < 1e15)
            {
                double lightYears = km / 9.4607e12;
                if (lightYears < 1000)
                    return $"{lightYears:F2} 光年";
            }

            if (km < 1e18)
            {
                double kiloLightYears = km / 9.4607e15;
                if (kiloLightYears < 1000)
                    return $"{kiloLightYears:F2} 千光年";
            }

            if (km < 1e21)
            {
                double megaLightYears = km / 9.4607e18;
                if (megaLightYears < 1000)
                    return $"{megaLightYears:F2} 百万光年";
            }

            // 最终单位：十亿光年（Gly）
            double gigaLightYears = km / 9.4607e21;
            return $"{gigaLightYears:F2} 十亿光年";
        }
    }
}