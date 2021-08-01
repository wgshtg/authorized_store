# 特約商店地圖

## 目的
使用 `WebApp` 來取代一般文件(Word, Excel or other base on cloud)的紀錄方式，提供更快速的瀏覽與查詢方式。

## 實作

### .NET Core

- .NET Core WebApp
    
    - Stores

        - 提供 `GET api/stores/{id}` 接口，依據 `id` 查詢商店。
        - 提供 `POST api/stores/query` 接口，依據 `StoreCriteria` 查詢商店。
        - 提供 `POST api/stores` 接口，新增商店。
        - 提供 `PUT/PATCH api/stores/{id}` 接口，修改指定 `id` 的商店
        - 提供 `DELETE api/stores{id}` 接口，依據 `id` 刪除指定 `id` 的商店。

    - Categories

        - 提供 `GET api/categories/{id}` 接口，依據 `id` 查詢類別。
        - 提供 `GET api/categories?{criteria}` 接口，依據 `name`、`pageIndex` 與 `pageSize` 查詢類別。
        - 提供 `POST api/categories` 接口，新增類別。
        - 提供 `PUT api/categories/{id}` 與 `PATCH api/categories/{id}` 接口，修改指定 `id` 的類別。
        - 提供 `DELETE api/categories/{id}` 接口，刪除指定 `id` 的類別。

    - Others
    
        - 透過 `AuthorizedStore.Fake` 模組注入假資料，並初始化。
        - 不合法字元正規表示式: ``[!@#$%^&*()`~\\-_=+\\[\\]\\{\\}\\\\|;:'\",.<>/?]+``。
