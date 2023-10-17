# Oblivion-Fortress Update Log

##10/17 from sankonsky
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

## 10/11 from kyle-ko
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
