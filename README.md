# Blackjack

基于 Unity 开发的 21 点（Blackjack）游戏。

## 项目简介

这是一个完整的 21 点游戏项目，包含发牌、要牌、停牌、结算、筹码、统计数据等核心玩法，并对游戏场景、音频、存档和资源加载进行了模块化设计。

## 核心功能

* **21 点核心玩法**

  * 玩家 / 庄家发牌
  * 要牌、停牌
  * Blackjack、爆牌等情况判定
  * 庄家按照规则自动要牌
  * 回合结算与筹码变化

* **游戏系统**

  * 多场景游戏流程
  * 音效与 BGM 音量控制
  * 游戏数据统计与 JSON 存档
  * 场景淡入淡出

* **资源与架构**

  * ScriptableObject 管理卡牌数据
  * Addressables 管理资源加载
  * Unity Event / GameEvent 实现系统间通信
  * DOTween 用于 UI 与场景过渡动画

## 操作

| 操作        | 功能    |
| --------- | ----- |
| 鼠标        | UI 操作 |
| Hit       | 要牌    |
| Stand     | 停牌    |
| Surrender | 投降    |

## 项目结构

```text
Assets/
├── Script/
│   ├── Card/       # 卡牌与牌组
│   ├── Player/     # 玩家相关逻辑
│   ├── Dealer/     # 庄家逻辑
│   ├── Manager/    # 游戏管理与数据
│   ├── UI/         # 游戏界面
│   └── Event/      # 游戏事件
├── ScriptableObject/
├── Prefab/
├── Scenes/
└── ...
```

## 技术

* Unity
* C#
* ScriptableObject
* Addressables
* DOTween
* Unity Event
* JSON 存档
* Unity Input System
