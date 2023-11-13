Build: .Net 7.0.402
施工中...(摸)

### 施工进度
- [x] 2023-10-26 添加 Utils.HttpClientFactory 生成 HttpClient.
- [x] 2023-10-26 修改WebClient为异步方式请求
- [ ] 2023-10-28 添加工具类：Logger等...(施工中)  
- [ ] ...

### 施工日志
- 2023-10-28
  - Logger 施工中
  - 调整了项目的文件结构
    - 新建Core文件夹。此文件夹将存放核心功能的源文件。
    - 将WebClient,HttpClientFactory从“Utils”转移到“Core/Web”，并将EncodeType从WebClient中分离。
    - 新增“Core/Logger”，作为存放日志记录相关功能类源文件
- 2023-10-29
  - Logger 施工中...
    - 完成Logger.LogManager.LogManager() -> 持续运行日志记录线程
    - 完成Logger.LogManager.WriteDown() -> 文件写入
    - ...
- 2023-11-13
  - ... 

### 参考: 
- https://github.com/leiurayer/downkyi
- https://github.com/SocialSisterYi/bilibili-API-collect