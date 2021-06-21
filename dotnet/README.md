# 特約商店地圖

## 目的
使用網頁來取代一般文件(Word, Excel)的紀錄方式，提供更快速的瀏覽與查詢方式

## 實作

### .NET Core

- .NET Core WebApp
    
    - Stores

        1. 提供 `GET api/stores/{id}` 接口，依據 `id` 查詢商店。
        2. 提供 `GET api/stores?{criteria}` 接口，依據 `pageIndex` 與 `pageSize` 查詢商店列表。
        3. 使用 `stores.json` 當作資料來源，於服務啟動時注入。

    - Categories

        1. 提供 `GET api/categories/{id}` 接口，依據 `id` 查詢類別。
        2. 提供 `GET api/categories?{criteria}` 接口，依據 `name`、`pageIndex` 與 `pageSize` 查詢類別。
        3. 提供 `POST api/categories` 接口，新增類別。
        4. 提供 `PUT api/categories/{id}` 與 `PATCH api/categories/{id}` 接口，修改指定 `id` 的類別。
        5. 提供 `DELETE api/categories/{id}` 接口，刪除指定 `id` 的類別。
