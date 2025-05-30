﻿using Strack.Model;

namespace Strack.External.Xingzhe.Model;


/// <summary>
/// 行者运动类型
/// </summary>
public enum XingzheActivityType
{
    /// <summary>
    /// 其他
    /// </summary>
    Other = 0,

    /// <summary>
    /// 徒步
    /// </summary>
    Hike = 1,

    /// <summary>
    /// 跑步
    /// </summary>
    Run = 2,

    /// <summary>
    /// 骑行
    /// </summary>
    Ride = 3,

    /// <summary>
    /// 游泳
    /// </summary>
    Swim = 5,

    /// <summary>
    /// 滑雪
    /// </summary>
    Ski = 6,

    /// <summary>
    /// 训练
    /// </summary>
    Workout = 8,

    /// <summary>
    /// 室内骑行
    /// </summary>
    IndoorCycling = 11,

    /// <summary>
    /// 虚拟骑行
    /// </summary>
    VirtualRide = 12,
}

public static class XingzheActivityTypeExtension
{
    public static ActivityType ToActivityType(this XingzheActivityType type)
    {
        return type switch
        {
            XingzheActivityType.Hike => ActivityType.Hike,
            XingzheActivityType.Run => ActivityType.Run,
            XingzheActivityType.Ride => ActivityType.Ride,
            XingzheActivityType.IndoorCycling => ActivityType.Ride,
            XingzheActivityType.VirtualRide => ActivityType.Ride,
            XingzheActivityType.Swim => ActivityType.Swim,
            XingzheActivityType.Ski => ActivityType.Ski,
            _ => ActivityType.Other
        };
    }
}