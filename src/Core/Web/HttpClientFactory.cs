namespace Core.Web {
    public class HttpClientFactory {
        private static readonly HttpClient httpClient;

        static HttpClientFactory() {
            var socketsHandler = new SocketsHttpHandler {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),  // * 连接池内连接生存时间为 2 分钟
                ConnectTimeout = TimeSpan.FromSeconds(300),          // * 连接建立 300 秒超时
                // CookieContainer =  
            };
            httpClient = new HttpClient(socketsHandler);
        }
        public static HttpClient GetHttpClient() {
            return httpClient;
        }
    }
}