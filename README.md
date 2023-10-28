Build: .Net 7.0.402
施工中...(摸)

### 施工日志
- [x] 2023-10-26 添加 Utils.HttpClientFactory 生成 HttpClient.
- [x] 2023-10-26 预计修改WebClient为异步方式请求
- [ ] 2023-10-28 添加工具类：Logger等...(施工中)
- [x] 2023-10-28 调整了项目的文件结构
  - 新建Core文件夹。此文件夹将存放核心功能的源文件。
  - 将WebClient,HttpClientFactory从“Utils”转移到“Core/Web”，并将EncodeType从WebClient中分离。
  - 新增“Core/Logger”，作为存放日志记录相关功能类源文件
- [ ] ...

### 参考: 
https://github.com/leiurayer/downkyi
https://github.com/SocialSisterYi/bilibili-API-collect