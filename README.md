# Tobii 眼動儀 C# 取得瞳孔數據

## 校正眼動儀

1. 請至 [Tobii Pro 官方網站](https://www.tobiipro.com/product-listing/eye-tracker-manager/)下載並安裝眼動儀校正軟體  

2. 插上眼動儀裝置至電腦上，將眼動儀放置電腦螢幕的下方，並打開 Eye Tracker Manager
![eye tracker manager.](/figure/eyeTracker-manager.jpg)

3. 將頭部與電腦保持適當距離，直至左側圖像呈現綠色，即可點選校正 (Calibrate) ，根據螢幕上出現的小點點，凝視該點直至該點爆破，重複幾次後就會顯示校正結果。而此結果會在每個校正區塊會有三個小點，分別為左眼、右眼、小點的座標位置。可以根據左右眼與小點的偏差，來決定是否需要重新校正，若沒有，則可以儲存離開。

    * 若更換受測者則需要重新校正。
    * 請參考 [校正步驟](http://developer.tobiipro.com/commonconcepts/calibration.html)


---

## 設定文字座標

* 開啟 `Setting Text.exe` 程式

    ![Setting Text.exe.](/figure/Setting%20Text.png)

    ![pc_test_area.](/figure/pc_test_area.png)


★ 在左側選擇文字排列方式 : 水平、垂直，輸出方式由左至右，由上至下。

★ 文字的起始座標 X、Y : 此處打 `(0,0)` ，指的是綠色區塊的 `(0,0)` 。

---

|按鈕與輸入框|功能介紹|
|-|-|
|內容|輸入文本內容|
|起始座標 X,Y|文本初始位置|
|新增頁數|清單最下方新增頁碼 #|
|>>|新增左邊文本至右側清單中|
|編輯|編輯清單中所選中的內容|
|刪除|刪除清單中所選中的內容|
|插入|在所選中的清單中前面插入當前左側的參數|
|清空 List|清除右側所有清單內容|
|存檔|將清單中所有內容存檔，選擇儲存之目錄，存成 `txt` 檔|
|查詢|查詢清單內是否有輸入方塊中的文字，若有則會將該筆資料反白|




---

## 紀錄座標點

* 在開啟程式前，請先將螢幕解析度調整成 `1920*1080`，且文字、應用程式大小調整成`100%`，如下圖所示，以`Win10`為例。

    ![win10 setting.](/figure/win10%20setting.png)

1. 設定完成後，開啟 `DrawText.exe`

    ![DrawText.exe.](/figure/DrawText%20UI.png)

2. 首先點選 『匯入文字檔』，選取剛剛輸出的文字座標txt檔或是程式內預設的文字檔，路徑為該程式的目錄下的 `txt` 資料夾內，有個 `test.txt`，將其匯入。此時程式會變成全螢幕，若想變更畫面大小可以按下列熱鍵 (HotKey)


|快捷鍵|用途|
|-|-|
|Ctrl + F11|`(Min)` 最小化程式介面|
|Ctrl + F12|`(Close)` 關閉程式|
|Ctrl + F1|`(Start)` 開始記錄眼動儀之座標點|
|Ctrl + F2|`(Stop)` 停止記錄|
|Ctrl + Delete|`(Hide)` 隱藏上方工具列|
|Ctrl + Insert|`(Show)` 顯示上方工具列|

3. 匯入文字完成後，會如下圖所示。


    ![import text.](/figure/DrawText%20import%20text.png)


4. 上方工具列可以調整頁數與文字的尺寸、字距、行距，設定完成後按下右側的提交按鈕，即可看到下方文字會依照您設定的參數做更動。


5. 調整頁數後按下提交，可以觀看其他頁面的文字，如下圖所示。


    ![page1.](/figure/page1.png) ![page2.](/figure/page2.png)

6.	點選匯入背景，能夠將所選圖片匯入至畫面背景中。

7.	當文字參數與眼動儀設備一切就緒後，則可以按下『`開始 !!`』或是快捷鍵 `Ctrl + F1`，開始記錄受測者所觀看螢幕的座標點。

    * 當按下『`開始 !!`』的瞬間，則會在該程式目錄下的『`csv`』資料夾下建立一個當前時間的 `EyeData.csv` 檔案，眼動儀會將數值不斷的新增至該 `csv` 檔案中。



8.	受測結束後，請按下『`End`』或是快捷鍵 `Ctrl + F2`，結束紀錄數值。
並且點擊『`儲存檔案`』，將當前的文字格式與各個文字座標紀錄下來，存至該程式目錄下的『`csv`』資料夾，檔名為當前時間 Text by page.頁數。


9.	至目前為止，程式會輸出兩個 `csv` 檔，為 `EyeData.csv`、`Text by page.1.csv`。
    *	`EyeData.csv` 檔案中，共紀錄了 :

        |欲取得之數據|輸出檔案 `csv` 的內容介紹|
        |-|-|
        |左右眼 X座標|Left X 、Right X ，以在螢幕上的百分比為單位|
        |左右眼 Y座標|Left Y 、Right Y ，以在螢幕上的百分比為單位|
        |左右眼 Z座標|Left Z 、Right Z |
        |時間戳記|TimeStamp |
        |左右眼瞳孔直徑|Left Diameter 、Right Diameter |

        詳細說明請見 :  [官方 Common concepts](http://developer.tobiipro.com/commonconcepts.html)

    *	`Text by page.1.csv` 檔案中，共紀錄了 :

        |欲取得之數據|輸出檔案 `csv` 的內容介紹|
        |-|-|
        |String|該文字內容|
        |Page|當前頁數|
        |X (LeftTop)|該文字的左上角座標點 X 軸，以像素 (px) 為單位|
        |Y (LeftTop)|該文字的左上角座標點 Y 軸，以像素 (px)  為單位 |
        |X (RightBottom)|該文字的左上角座標點 X，以像素 (px)為單位 |
        |Y (RightBottom)|該文字的左上角座標點 Y，以像素 (px) 為單位 |
        |FontSize|該文字的尺寸 |
        |LetterSpace|該文字的字距 |
        |LineSpacing|該文字的行距 |
        

---


## 配對座標點

* 將 `EyeData.csv` 與 `Text.csv` 兩個檔案做配對，找出該文字之凝視時間、該文字在螢幕上的座標與百分比、瞳孔直徑。

    1. 開啟 `Match.exe`

        ![match.](/figure/match.png)

    2.	點擊『`EyeData`』選取剛才測試後的 `EyeData.csv`。
    3.	點擊『`TextData`』選取剛才測試後的 `Text.csv`。
    4.	點擊『`Submit`』提交，並選擇要匯出的路徑。
    5.	該檔名為 `output` 當前時間。

* 下圖為比對後的結果

    `Excel` 檔案內容介紹

    ![match result.](/figure/match_result.png)

    |標題|介紹|
    |-|-|
    |String_X (px)|該文字於螢幕之所在位置X軸，以像素為單位|
    |String_Y (px)|該文字於螢幕之所在位置Y軸，以像素為單位|
    |Eye_X (%) |該文字於螢幕之所在位置X軸，以百分比為單位|
    |Eye_Y (%) |該文字於螢幕之所在位置Y軸，以百分比為單位|
    |Eye_Z|觀看該文字時，與眼動儀之間的距離|
    |Eye_D|觀看該文字時，瞳孔之直徑|
    |TimeStamp|觀看該文字所花費的時間，以秒為單位|
    





