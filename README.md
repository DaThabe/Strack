
![banner](Res/Design/LOGO.png)

# Strack

[![version](https://img.shields.io/badge/Version-0.0.1alpha-blue.svg)](https://github.com/DaThabe/Strack/releases)
[![MIT License](https://img.shields.io/badge/license-MIT-green.svg)](https://github.com/DaThabe/Strack/blob/master/LICENSE.txt)

Strack 是一款基于 WPF 开发的桌面应用程序，专注于运动轨迹的展示与可视化分析。无论是日常跑步、骑行、徒步等轨迹数据，都能提供直观、清晰的地图可视化和多维度的数据统计支持。

项目灵感来源于优秀的开源项目 [running_page](https://github.com/yihong0618/running_page)，Strack 支持轨迹导入、地图展示、轨迹详情查看及后续拓展能力。

项目名称 Strack 源于 Super Track 的组合灵感，寓意对轨迹的全面掌控与深度探索。

## ✅ 主要功能

- 📥 导入轨迹数据：支持通过 API 或标准格式（如 GPX）导入运动轨迹
- 🗺️ 地图可视化：基于 Mapsui 实现地图展示，可显示全部轨迹或单次活动路线
- 📊 运动数据分析：自动统计运动总里程、时长、配速等核心指标
- 🧭 轨迹详情页：每条轨迹支持单独查看详细时间、路线、类型等信息
- 🌙 多主题支持（开发中）

## 🚀快速开始

- **使用发行版**：可前往 [Releases](https://github.com/DaThabe/Strack/releases) 页面下载最新版。
- **自行构建**： 安装 .NET 10 Runtime 后，使用 Visual Studio 2022 或更高版本打开 Strack.sln 进行调试或开发。

## 📷 界面截图

[!img1](/)

## 🧰 技术栈

| 项目 | 用途 | 协议 |
| :-- | :-- | :-- |
| [dotnet/runtime](https://github.com/dotnet/runtime) | .NET 9 核心运行时 | MIT |
| [dotnet/wpf](https://github.com/dotnet/wpf) | 桌面 UI 框架 | MIT |
| [dotnet/efcore](https://github.com/dotnet/efcore) | ORM 数据访问层 | MIT |
| [Microsoft.Extensions.Hosting](https://github.com/dotnet/runtime) | 宿主与生命周期管理 | MIT |
| [Microsoft.Xaml.Behaviors.Wpf](https://github.com/Microsoft/XamlBehaviorsWpf) | 行为与触发器支持 | MIT |
| [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) | MVVM 框架 | MIT |
| [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) | JSON 序列化与反序列化 | MIT |
| [UnitsNet](https://github.com/angularsen/UnitsNet) | 单位换算与物理量支持 | MIT |
| [Mapsui.Wpf](https://github.com/Mapsui/Mapsui) | 地图显示与投影支持 | LGPL-2.1 |
| [CalcBinding](https://github.com/Keboo/CalcBinding) | 表达式绑定增强 | MIT |

## 📌 开发计划

- [1] GPX 文件导入支持
- [ ] Dark / Light 主题切换
- [ ] 多用户数据管理

## 🗂️ 项目信息

![Alt](https://repobeats.axiom.co/api/embed/d64be33fe7e3cde9376f47827202e35846a5de21.svg "Repobeats analytics image")

## 🏆 贡献者

[![Toolkit Contributors](https://contrib.rocks/image?repo=Dathabe/Strack)](https://github.com/DaThabe/Strack/graphs/contributors)

## 📊 图表与徽章工具

| 名称 | 作用 |
| :-- | :-- |
| [shields.io](https://shields.io/) | 用于生成开源协议、版本等项目徽章 |
| [repobeats.axiom.co](https://repobeats.axiom.co) | 展示项目提交活动与热度趋势图 |
| [contrib.rocks](https://contrib.rocks) | 可视化展示 GitHub 贡献者头像墙 |
