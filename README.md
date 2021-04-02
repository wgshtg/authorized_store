# 特約商店地圖

## 目的
使用網頁來取代一般文件(Word, Excel)的紀錄方式，提供更快速的瀏覽與查詢方式

## 實作

### .NET Core

- .NET Core WebApp
    
    1. 提供 `GET {base_url}/api/stores/{id}` 接口，依據 `id` 查詢商店。
    2. 提供 `GET {base_url}/api/stores?{criteria}` 接口，依據 `pageIndex` 與 `pageSize` 查詢商店列表。
    3. 使用 `stores.json` 當作資料來源，於服務啟動時注入。
