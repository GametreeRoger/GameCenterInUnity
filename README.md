# GameCenterInUnity
  - unity 接 swift的範例是參考自(https://github.com/miyabi/unity-swift)
  - 使用swift在Unity裡使用GameCenter 儲存遊戲進度的功能.
  - 可以直接使用"Package"資料夾裡的"GameCenterInUnity.unitypackage"Import到空專案中

## 注意事項
 - Unity 的 "Player Setting"裡
 - ![Imgur](http://i.imgur.com/NLFQtkF.png)
  - "Company Name"請改成自己的
  - "Other Settings"裡
    - "Bundle Identifier"請改成自己的
    - "iOS Developer Team ID" 請填自己的，(https://developer.apple.com 到這裡登入自己帳號去"Membership"就可以看到自己的Team ID了)
    - ![Imgur](http://i.imgur.com/4Esp8cc.png)
    - "Target SDK" 放到手機上的話就選"Device SDK", 放到模擬器的話就選“Simulator SDK"
    - "Target minimum iOS Version"要選8.0
---
- iTruns Connect
  - 把自己的遊戲的 GameCenter選項打開
---
- XCode的"Capabilities"
  - 開啟iCloud
    - key-value storage 開啟
    - iCloud Documents 開啟
      - iCloud services 要選iCloud Documents的原因 (http://stackoverflow.com/questions/28563908/cant-savegamedata-because-not-signed-in-to-icloud-although-i-am-signed)
  - 開啟Game Center

## 使用說明
- Unity
  - Plugins/iOS/ 裡的兩個檔案是接GameCenter的檔案code就不多說了，可以參考這裡知道怎麼接就OK了(https://github.com/miyabi/unity-swift)
  - UnitySwift/Editor/PostProcessor.cs
    - 這是自動設定Xcode的檔案裡面有幾個重點
      - 設定Info.plist裡的"UIRequiredDeviceCapabilities"把armv7和gamekit加入
      - 產生[自己專案的名稱].entitlements這個檔案，這個檔案是紀錄Capabilities裡面iCloud裡面我們要選的選項
      - 修改project.pbxproj這個檔案裏面的字串來開啟iCloud功能，參考自(https://github.com/jespertheend/unity-post-build-script/blob/master/build.cs)
        - 裡面有幾個常數需要換成自己的資訊ENTITLEMENTS_NAME 和ENTITLEMENTS_REFERENCE_CODE
          - ENTITLEMENTS_NAME 就是把自己專案名稱全部小寫後面再加上".entitlements"
          - 和ENTITLEMENTS_REFERENCE_CODE 需要把PostProcessor.cs功能先關掉自己手動去XCode把iCloud的功能開啟一次(可以把[PostProcessBuild]這行先註解掉就不會被執行了)底下XCode區域會解說怎麼找這個字串

- XCode
  - ![Imgur](http://i.imgur.com/CmQPAvg.png)
  - 手動把這些功能開啟之後就會看到產生了一個[自己專案的名稱].entitlements的檔案，這也是在PostProcessor裡面要自己產生的檔案
  - ![Imgur](http://i.imgur.com/Zzkr1tI.png)
  - 再到XCode的專案資料夾找"Unity-iPhone.xcodeproj"用下圖的方式打開
  - ![Imgur](http://i.imgur.com/QCJ5r4o.png)
  - 就會看到"project.pboxproj"這個檔案
  - ![Imgur](http://i.imgur.com/mT7CJza.png)
  - 用文字編輯器打開後尋找"/\* CustomTemplate \*/ = {"這個字串，應該就會看到[自己專案的名稱].entitlements的前面有一串字串那就是ENTITLEMENTS_REFERENCE_CODE.

- 把PostProcessor.cs裡的兩個常數都填完後記得檢查一下你的Player setting裡的other settings有沒有填好，不知道怎麼填請看上面的"注意事項"
- 基本上把這樣build出來就可以執行了.
