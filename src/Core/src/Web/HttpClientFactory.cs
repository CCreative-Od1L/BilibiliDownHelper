using System.Net;

namespace Core.Web {
    public class HttpClientFactory {
        private static readonly HttpClient httpClient;
        static HttpClientFactory() {
            var socketsHandler = SocketsHttpHandlerFactory(2, 300);
            httpClient = new HttpClient(socketsHandler);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pooledConnectionLifetime">单位：分钟</param>
        /// <param name="connectTimeout">单位：秒</param>
        /// <param name="cookieContainer"></param>
        /// <returns></returns> <summary>
        static SocketsHttpHandler SocketsHttpHandlerFactory(
            uint pooledConnectionLifetime,
            uint connectTimeout) {
                return new (){
                    PooledConnectionLifetime = TimeSpan.FromMinutes(pooledConnectionLifetime),
                    ConnectTimeout = TimeSpan.FromSeconds(connectTimeout),
                    UseCookies = false,     // * 禁止自动处理Cookie
                };
        }
        public static HttpClient GetHttpClient() {
            return httpClient!;
        }
    }
}