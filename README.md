# Oblivion-Fortress Update Log
## 10/11 form kyle-ko
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

## 10/9 from Sankonsky
- 地圖邊界
- 樹&碰撞區塊

## 10/8 from Sankonsky
- 建立isometric tilemap遊戲場景
