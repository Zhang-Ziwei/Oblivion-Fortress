# Oblivion-Fortress Update Log

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
  
## 10/18 from sankonsky
### first demo version
- 調整鍵盤配置:
  - Ctrl:衝刺
  - Space:採集
  - E:撿起/放下
- 已知問題:
  - Enemy.cs的using UnityEditor.UI會造成遊戲無法輸出，先暫時改為註解處理，目前在遊戲中正常
  - (已修復)CollectResource.cs的長按鍵計時器在測試版及網頁版相差極大，暫時先把count由500調成50，可能是幀數不同造成的問題
  - 衝刺的時間在測試版及網頁版也相差極大，很可能也是幀數不同造成的問題
  - 衝刺會無視collision衝出地圖外

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
