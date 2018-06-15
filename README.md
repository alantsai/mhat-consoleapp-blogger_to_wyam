# mhat-consoleapp-blogger_to_wyam

這個專案提供一個以C# sample的console程式用作與處理Google Blogspot（Blogger）匯出文章的xml，
方便搬遷部落格到別的平臺。

主要提供兩個部分的處理：
1. 把文章轉換成爲chstml，并且處理幾個部分
    - 圖片位置從本來連到google photo改成local的位置
    - 處理tag標簽的網址
    - 把js高亮的套件從syntachighlighter改成prismjs
2. 產生出放在blogger的xml内容，方便seo用途

詳細可以參考我的部落格系列：[部落格改版學DevOps](http://blog.alantsai.net/tags/%E3%80%8C%E9%83%A8%E8%90%BD%E6%A0%BC%E6%94%B9%E7%89%88%E5%AD%B8devops%E3%80%8D)
裡面的實際搬遷篇

## 快速使用 (getting started)

目前這個專案屬於sample程式碼，因此會需要自己手動clone下來透過visual studio執行（如果有人有需要編譯過的版本可以直接使用
請發issue或者在部落格留言給我）

### 直接使用

把solution build之後，透過command line的方式傳入三個參數：
1. `-p`: 從blogger匯出的xml路徑
2. `-i`: 從google photo備份的圖片路徑
3. `-o`: 最後產出的cshtml路徑

例如：
```powershell
.\MHAT.BloggerToWyam.ConsoleApp.exe -p "d:\Library\Downloads\blog-03-22-2018.xml" -i "d:\Library\Downloads\blog\
image\" -o "d:\Library\Downloads\blog\post\"
```

### 主要程式碼調整位置

這個軟體的主要邏輯流程在：[Bll/Process/XmlBackupProcess.cs](/src/MHAT.BloggerToWyam.ConsoleApp/Bll\Process\XmlBackupProcess.cs)

整個程式碼的流程應該看就能夠懂，不過大概做以下幾個事情：
1. 把blogger匯出的xml反序列化成爲物件
2. 取得是文章的部分
3. 針對每一個文章
    - 處理圖片
    - 處理tag網址
    - 處理語法高亮（syntax highlight）從本來的syntaxhighlighter改成prismjs
4. 匯出產出的cshtml以及圖片
5. 在console印出應該放在blogger的xml内容

整個的Console產出有3個部分：
1. 那些處理完成
2. 那些處理失敗
3. 應該放在blogger作爲seo的xml内容

每一個區域以多個等號（`========`）區隔，一個沒問題的範例截圖如下，可以看到處理失敗的部分是空

![產出返利](/docs/assets/output-with-no-error.png)

### 注意事項

假設有出現錯誤，最有可能的原因是因爲圖片domain導致。在google的圖片網址的subdomain都不一樣，程式裡面有透過一個
dictionary把他們都一致化，但是還是有可能有些沒處理到，可以參考 [Model/BlogImage.cs](/src/MHAT.BloggerToWyam.ConsoleApp/Model/BlogImage.cs)
裡面的`OriginalUrlWithDomainFix`

## 幫助 (support)

如果有任何問題，可以透過[開issue](https://github.com/alantsai/mhat-consoleapp-blogger_to_wyam/issues/new)。

## 參與修改 (Contributing)

歡迎任何形式的參與，更多資訊請參考  
[CONTRIBUTING.md](CONTRIBUTING.md)

## 授權 (License)

本專案屬於 MIT License，更多資訊請看 [LICENSE.md](LICENSE.md)
