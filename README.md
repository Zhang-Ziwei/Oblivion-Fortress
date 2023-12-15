# Oblivion-Fortress Update Log

## 12/14 from sankonsky
- 無盡模式地圖 Infinite
- 關卡目錄
- 遊戲結束音效

## 12/14 from sankonsky
- 修復放下工具後角色動畫卡住的問題 :)
- 修復背景音效的重播問題
- 已知bug:
  - 敵人數量有機率變成負數

## 12/13 from sankonsky
- 隨機生成資源 GenerateResources.cs
  - 掛在Resources上，可調整生成時間間距

## 12/12 from kyleko56
- Level 3 防禦塔初步平衡
- chapter 2 關卡設計
   - 資源不足所以先複製幾份
- 調整 towerinfo UI
- 調整經驗值機制 (每座塔分開解鎖)
- 其他改動
   - 讓地基不會因重疊到木材、石頭、人物等而不能設置
   - 放錯物資到地基現在變成會放在旁邊
- bug: 後面波數若召喚與前面波數相同 ID 的敵人不會召喚在出生點，並且 HP 為 0
   - 我先 comment 掉 EnemySummon.cs 第82行

## 12/11 from sankonsky
- 微調level UI
- 修改難度按鈕外觀，新增變色功能

## 12/9 from henryhuang920712
- 新增buff
   - Resurrection (復活): 原地復活一次
   - Summon (召喚): 隨機召喚若干其他敵人
   - Healing (治癒): 治療範圍內敵人
- 修改buff 架構，現在分成攻擊事件 (AttackEvent) 跟死亡事件 (DeathEvent)

## Third demo 測試回饋
- https://docs.google.com/forms/d/1e2zId1f4n8VI6e1pTg5O81HXozueyTJQ89Pq7YrobeE/edit

## Third demo version
- 已知bug:
  - 角色被擊/中毒後腳步動畫故障
  - 切換手中工具後人物圖片未改變
  - 拿取建材後無法翻滾/攻擊

## 12/5 from kyleko56
- 完成防禦塔升級機制及 UI
  - 包含經驗值、解鎖
- 重新平衡製作 Chapter 1

## 12/4 from sankonsky
- 新增場景
  - 第三關 Chapter3
- 完成第三關地圖布置(tilemap、resources、path)
- 更改木材、石材圖案
- 新增砍樹&敲石頭音效
- 完成新手教學關卡
- 優化撿取系統
  - 現在手持物品時，按一次E鍵即可和地上的物品切換，不需要先放下再撿取了
- 已知bug:
  - 被紫色怪物打到有機率卡死
- 已修復問題:
  - buildingUI圖層顯示錯誤

## 11/29 from sankonsky
- 新增場景
  - 關卡選擇 List
  - 新手教學關 Tutorial
  - 第二關 Chapter2
- 完成第二關地圖布置(tilemap、resources、path)
- 已知bug:
  - 選蓋第一種塔時，蓋好會出現第三種塔

## 11/28 from sankonsky
- 復活機制，每次死亡懲罰時間由10s,20s,30s,...遞增
- 新增玩家受擊&死亡動畫
- 已知bug:
  - 玩家切換工具動畫延遲過久
  - 持有木材或石頭時玩家動畫會卡死
- 已修復問題:
  - 敵人攻擊主堡時會同時扣玩家血量
  - 按O,P鍵會躺平暴斃


## 11/24 from henryhuang920712
 - 新增音效
     - 四種敵人攻擊、受傷、死亡的音效
     - 音效延遲可以到`Prefabs\Enemies`的Audio那欄調整
 - 解決freeze的bug
     - buff那邊增加cooldown time可以填寫
 - 新增傷害UI

## 11/22 from kyleko56
- 新增音效
  - 目前有背景音樂、主堡受擊、敵人出擊、建築完成
- 新增防禦塔資訊、升級 UI
- 新增防禦塔
  - 毒、冰、回復、加攻、對同一敵人持續增加傷害
- 修復 bugs
  - 物品不易撿起
  - 斜向移動較快
- 已知 bug: 角色 freeze 結束後仍不能移動

## 11/18 from henryhuang920712
  - 修復Debuff無法新增Bug
    - 改成將`Scripts\Enemies\EnemyBuff`當中的Script拉到`Prefabs\Enemies`當中
    - 記得填寫Duration、Interval等數據
  - 完成Debuff UI System
    - 新增Freeze
  - 拉下來新版本的時候發現**防禦塔無法正常顯示**，在我之前有人去動到防禦塔的sprites嗎？

## 11/14 from Zhang-Ziwei
- 工具重叠修復
  - 撿起放下系統工具已無法重叠
- 衝刺出地圖修復
- 新增持斧動畫
  - 持斧休息（無按鍵）
  - 持斧跑步（WASD）
  - 持斧衝刺（SHIFT）
  - 持斧三連擊（滑鼠左鍵）
- 新增持錘動畫
  - 持錘休息
  - 持錘跑步
  - 持錘衝刺
  - 持錘三連擊
- 新增持劍動畫
  - 持劍休息
  - 持劍跑步
  - 持劍衝刺
  - 持劍三連擊

## 11/11 from henryhuang920712
- 新增敵人
  - Evil Wizard (ID: 6)
- 新增Debuff基本功能
  - 中毒持續扣血 (ID: 0)
  - 緩速玩家 (ID: 1)
  - 相關設定在`Assets/Resources/Enemies/EnemyBuffData`中
  - 可以到`Assets/Prefabs/Enemies`中設定每個敵人的Debuff
  
## mid demo 測試回饋 
- https://docs.google.com/forms/d/1XlGyXq3QoD98YvhbscSh1o9Opyu7uBL1_A-EkI56SDc/edit#responses

## mid demo version

## 11/9 from kyleko56
- 新增防禦塔資訊 UI
- 已修復bug: 玩家斜向速度較快

## 11/8 from sankonsky
- 修改城堡及蝙蝠血量
- 已修復bug:
  - 網頁版gameover後遊戲卡死
  - 蓋塔時玩家血量顯示異常

## 11/8 from henryhuang920712
- 已修復bug:
  - 重新遊戲後怪物無法正常生成
  - 進攻回合無法正常運作
  
## 11/7 from kyleko56
- 關卡設計
- 將資源、敵人、玩家、工具、主堡的 display layer 都設成 5
- 目前波數 UI
- 修復 bug: rush 時間不一致 & 東西會撿起來馬上放下
- 已知 bug: 蓋塔時向左右跑會讓玩家血量顯示異常

## 11/7 from sankonsky
- 新增GameOverUI/WinUI
- 新增採集量條，顯示目前採集進度
- 道路判定:現在防禦塔無法蓋在道路上
- 玩家血量機制 HPControl.cs
  - 血量量條
  - 敵人攻擊玩家會扣血
  - 目前不會復活，血量歸零即結束遊戲
- 待解決問題:最後一波進攻到一半系統就判定獲勝，且獲勝後若重新進行遊戲不會生成怪物(死亡後重新開始不會有問題)

## 11/6 from henryhuang920712
- 敵人攻擊主堡
    - 主堡血量設定
- 玩家攻擊敵人
    - 玩家攻擊力、攻擊範圍設定
- 關卡開始前讀秒 (Before Spawn Interval)
    - 設定在`Assets/Resources/Enemies/EnemyLevelData`中
- 新敵人
    - Bringer Of Death (ID: 5)

## 11/6 from sankonsky
- 撿起系統 PickupSystem.cs
  - 持有狀態: 0空手/1斧/2鎬/3木頭/4石頭
  - 須持對應工具才可採集
  - 目前持有物品顯示在左上角，之後再改成以動畫呈現
  - 若角色在建築物地基上，放下材料會直接放入地基
- 移除DepositMaterial.cs，功能合併至撿起系統
- (已修復)~~待解決問題:撿起與放下會同時判定，暫時先分開成兩個按鍵，E撿起R放下~~
- 修復問題:原本按E角色會直接躺平，目前把躺平鍵移到O，但還不知道如何移除躺平功能...

## First Demo 測試回饋
- https://docs.google.com/forms/d/1vdaiac8mL2BVGx3HF-6avDuczlM2sxjXHwt9O9j7L4s/edit#responses

## 11/5 from sankonsky
- 新增主堡外觀
- 衝刺鍵由Ctrl改成Shift
- 建造防禦塔UI
- 採集後會掉落對應材料，樹->木材，石頭->石塊
  - 石塊外觀還沒找到適合的

## 11/5 from kyleko56
- 新增緩速防禦塔
- 更新防禦塔外觀、攻擊動畫

## 10/30 from henryhuang920712
- 新增兩種敵人
  - Mushroom (ID: 3)
  - Flying eyes (ID: 4)
- 完成追蹤玩家相關程式
  - 可以到`Assets/Resources/Enemies/`當中設定每個敵人的玩家追蹤範圍 (Detect Range)、攻擊範圍 (Attack Range)
  - 攻擊動畫、被攻擊動畫、死亡動畫

## 10/25 from sankonsky
- 新增石頭
- 把樹和石頭從tilemap座標系改成一般座標系

## 10/25 from Zhang-Ziwei
- 衝刺出地圖修復
- 撿起斧頭動畫修復
- 新增衝刺動畫
- 連續衝刺速度過快修復，目前只有一次衝刺完成后才能進行下一次衝刺
  
## 10/18 from sankonsky
- 新增封面圖
- 新增角色空手/拿斧頭圖片
- 新增主堡判定區域(還沒放圖)
- 新增暫停選單，按Esc觸發

## 10/18 from kyleko56
- 讓 UI 能在不同解析度顯示
- 增加開始畫面、restart按鈕
  
## first demo version
- 已知問題:
  - Enemy.cs的using UnityEditor.UI會造成遊戲無法輸出，先暫時改為註解處理，目前在遊戲中正常
  - (已修復)CollectResource.cs的長按鍵計時器在測試版及網頁版相差極大，暫時先把count由500調成50，可能是幀數不同造成的問題
  - 衝刺的時間在測試版及網頁版也相差極大，很可能也是幀數不同造成的問題
  - 衝刺會無視collision衝出地圖外

## 10/18 from sankonsky
- 調整鍵盤配置:
  - Ctrl:衝刺
  - Space:採集
  - E:撿起/放下

## 10/18 from kyleko56
- 簡單版物資蒐集 (砍掉便獲得物資，沒有掉落物)
- 關卡設計

## 10/17 from kyleko56
- 防禦塔外觀、攻擊動畫
- 新增一種防禦塔
- 刪除防禦塔、地基按鈕
  
## 10/17 from sankonsky
- 合併branch

## 10/16 from henryhuang920712
- 完成關卡流程的設置
  - 關卡流程的設置在`Assets/Scripts/LevelManager.cs`
  - 關卡敵人的資料在`Assets/Resources/Enemies/EnemyLevelData`中
  - 目前共有2關，2關結束後會直接結束遊戲
- 完成敵人外殼與基本動畫
  - Skeleton (ID: 1)
  - Goblin (ID: 2)
- 發現未知Bug
  - `unity ArgumentNullException: Value cannot be null. Parameter name: _unity_self`
  - 解決方法：reload the scene

## 10/15 from Zhang-Ziwei
- 摄像头随人物移动而移动
- 人物冲刺

## 10/12 fron sankonsky
- Debug
  - 修正tilemap
  - 修正UI及canva位置
  - 修正鏡頭
- 砍樹機制 (./Player/CollectResource)
  - 在樹旁邊長按F鍵可以砍樹
  - 未完成:砍完樹後掉落木材

## 10/11 from kyleko56
- 完成項目
  - 防禦塔建造&攻擊
    - 放置地基時顯示射程
    - 地基已放置物資數量UI (目前為按一次 E 就放置一個木頭和石頭)
    - 地基不能與其他物件 collider 重疊
  - Display Layer
    - 0: tilemap
    - 1: 防禦塔攻擊範圍
    - 2: 地基
    - 3: 放置地基時的紅色地基UI
    - 4: Player
    - 5: 防禦塔
    - 100: UI  
- 待做
  - 道路的 tile 可能要加 collider，不然地基可以建在上面 
  - 防禦塔攻擊動畫
  - 刪除防禦塔、地基
- Note
  - 如果跳出 Import TMPEssential 的視窗要 import 它
  - GameData 這個 script 中有一些跟距離有關的 function，可能會有用
  - Button 是為了方便暫時放在中間

## 10/9 from sankonsky
- 地圖邊界
- 樹&碰撞區塊

## 10/8 from sankonsky
- 建立isometric tilemap遊戲場景
