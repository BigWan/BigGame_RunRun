# RunRun

## 概述

**个人项目,学习Unity练手**

## 动作状态机


>@后缀表示loop

角色用UnityChan 现成的资源,包括模型和动作.

    受伤@
    卧倒起立
    被击倒
    跳跃中@
    起跳
    跳跃最高点
    下落
    下跪
    下跪起身
    跑@
    行走@
    胜利
    站立


## 功能点TODO

0. 角色控制
    * 动画状态机
    * Camera跟随
    * 入场/死亡等行为逻辑
1. 跳跃障碍功能
    * 越过障碍
    * 碰撞障碍
2. 躲避敌人功能
    * 躲避敌人
    * 碰撞敌人减速，重新加速
3. 左右位移变道
    * 没有合适的位移动画需要配合特效制作功能
    * 位移残影
    * 极限躲避障碍
4. 道具功能
    * 金币
    * 磁铁
    * 无敌
    * 能量
    * 恢复
    * 飞行道具
5. 得分计算
    * 各个功能得分点
6. UI
7. 完整游戏流程
8. PCG生成关卡，场景制作
    * 关卡内容
    * 场景内容


## 系统设计

### 跑道设计思路


- 每个跑酷关卡含有一个`Track`
- 每个`Track`由若干个段落组成`RoadSection`
- `RoadSection`又由若干个`Block`组成

> Block是跑道的最小单位,一个Block的标准长度是 `3.75` 或者是 `7.5` ,宽度可能是1-3个道.存在空的Block,其长度也为`3.75`


* Track
    * RoadSection1
        * Block1
        * Block2
        * ...
    * RoadSection2
    * RoadSection3
    * RoadSection4
    * ...

`RoadSection`通过`RoadSectionData`来生成具体的段落.

`RoadSectionData`由`命令(SpawnBlockCommand)`列表构成.一个生产命令会产生若干个Block.一个RoadSection只会有一个RoadSectionData.
RoadSection在生成的时候会有一些细小的随机过程.每个Block实例在细节上也不尽相同.


整个Track的结构就是这样.在生产管卡Track的时候,关卡根据自身的Track配置,从一组RoadSectionData中随机选择Data来生成Section.


Track又分为无尽Track和有限Track

- 有限Track需要额外配置 `StartSection` 和`EndSection`.
- 无尽Track不需要`EndSection`


关卡内容的制作主要是配置`RoadSectionData`


### 道具系统


### 障碍系统


### 计分系统





### 移动控制

人物移动有两种行为:
1. 变道路,不改变方向,只改变side
    左
    中
    右
2. 变朝向,改变方向,重置side
    左转
    右转