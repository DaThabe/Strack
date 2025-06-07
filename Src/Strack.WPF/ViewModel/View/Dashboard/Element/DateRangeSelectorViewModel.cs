using CommunityToolkit.Mvvm.ComponentModel;
using Strack.Desktop.ViewModel.Shell.Navigation.Page;
using Strack.Model.Setting;
using Strack.Service;
using Strack.Service.Setting;

namespace Strack.Desktop.ViewModel.View.Dashboard.Element;

public partial class DateRangeSelectorViewModel(
    ISetter<DateRangeSelectorViewModel> setting
    ) : ObservableObject
{
    /// <summary>
    /// 范围类型
    /// </summary>
    [ObservableProperty]
    public required partial DateRangeType Type { get;  set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [ObservableProperty]
    public required partial DateTimeOffset BeginTime { get;  set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [ObservableProperty]
    public required partial DateTimeOffset EndTime { get;  set; }


    partial void OnTypeChanged(DateRangeType value)
    {
        if (value == DateRangeType.All)
        {
            BeginTime = DateTimeOffset.MinValue;
            EndTime = DateTimeOffset.MaxValue;
        }
    }

    partial void OnBeginTimeChanged(DateTimeOffset value)
    {
        if (Type == DateRangeType.Week)
        {
            EndTime = BeginTime.AddDays(7);
        }
        else if (Type == DateRangeType.Month)
        {
            EndTime = BeginTime.AddMonths(1);
        }
        else if (Type == DateRangeType.Year)
        {
            EndTime = BeginTime.AddYears(1);
        }
    }

    partial void OnEndTimeChanged(DateTimeOffset value)
    {
        if (Type == DateRangeType.Week)
        {
            BeginTime = EndTime.AddDays(-7);
        }
        else if (Type == DateRangeType.Month)
        {
            BeginTime = EndTime.AddMonths(-1);
        }
        else if (Type == DateRangeType.Year)
        {
            BeginTime = EndTime.AddYears(-1);
        }
    }




    public async ValueTask InitializeAsync(CancellationToken cancellationToken)
    {
        //Type = await settingService.GetRequiredAsync<DateRangeType>(SettingPath.FromMember<DateRangeSelectorViewModel>(nameof(Type)), DateRangeType.All);
    }



    /// <summary>
    /// 自定义范围
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    public void Custom(DateTimeOffset begin, DateTimeOffset end)
    {
        Type = DateRangeType.Custom;
        BeginTime = begin;
        EndTime = end;
    }

    /// <summary>
    /// 下一周
    /// </summary>
    /// <param name="begin"></param>
    public void NextWeek(DateTimeOffset begin)
    {
        Type = DateRangeType.Week;
        BeginTime = begin;
        EndTime = begin.AddDays(7);
    }

    /// <summary>
    /// 上一周
    /// </summary>
    /// <param name="end"></param>
    public void LastWeek(DateTimeOffset end)
    {
        Type = DateRangeType.Week;
        BeginTime = end.AddDays(-7);
        EndTime = end;
    }

    /// <summary>
    /// 下一月
    /// </summary>
    /// <param name="begin"></param>
    public void NextMonth(DateTimeOffset begin)
    {
        Type = DateRangeType.Week;
        BeginTime = begin;
        EndTime = begin.AddMonths(1);
    }

    /// <summary>
    /// 上一月
    /// </summary>
    /// <param name="end"></param>
    public void LastMonth(DateTimeOffset end)
    {
        Type = DateRangeType.Week;
        BeginTime = end.AddMonths(-1);
        EndTime = end;
    }



    /// <summary>
    /// 上一个周期
    /// </summary>
    /// <param name="end"></param>
    public void Prev(DateTimeOffset end)
    {
        if (Type == DateRangeType.Week)
        {
            BeginTime = end.AddDays(-7);
            EndTime = end;
        }
        else if (Type == DateRangeType.Month)
        {
            BeginTime = end.AddMonths(-1);
            EndTime = end;
        }
        else if (Type == DateRangeType.Year)
        {
            BeginTime = end.AddYears(-1);
            EndTime = end;
        }
    }

    /// <summary>
    /// 下一个周期
    /// </summary>
    /// <param name="begin"></param>
    public void Next(DateTimeOffset begin)
    {
        if (Type == DateRangeType.Week)
        {
            BeginTime = begin;
            EndTime = begin.AddDays(7);
        }
        else if (Type == DateRangeType.Month)
        {
            BeginTime = begin;
            EndTime = begin.AddMonths(1);
        }
        else if (Type == DateRangeType.Year)
        {
            BeginTime = begin;
            EndTime = begin.AddYears(1);
        }
    }

}

public enum DateRangeType
{
    /// <summary>
    /// 全部范围
    /// </summary>
    All,

    /// <summary>
    /// 周期范围
    /// </summary>
    Week,

    /// <summary>
    /// 月份范围
    /// </summary>
    Month,

    /// <summary>
    /// 年份范围
    /// </summary>
    Year,

    /// <summary>
    /// 自定义
    /// </summary>
    Custom
}