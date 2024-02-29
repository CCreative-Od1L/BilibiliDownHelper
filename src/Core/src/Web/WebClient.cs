using System.Net;
using System.Text;
using System.Net.Http.Headers;
using System.IO.Compression;
using Core.Utils;
using Microsoft.VisualBasic;
using Core.CookieFunc;
using Core.BilibiliApi.User;
namespace Core.Web {
    // * internal
    public static class WebClient {
        public static ENCODE_TYPE CheckEncodingType(string contentEncoding) {
            string lowerContent = contentEncoding.ToLower();
            if (lowerContent.Contains("gzip")) {
                return ENCODE_TYPE.GZIP;
            } else if (lowerContent.Contains("deflate")) {
                return ENCODE_TYPE.DEFLATE;
            } else if (lowerContent.Contains("br")) {
                return ENCODE_TYPE.BR;
            }
            return ENCODE_TYPE.DEFAULT;
        }
        public static void BatchAddHeaderAcceptLanguage(
            HttpRequestMessage requestMessage,
            Tuple<string, double>[] languages
        ) {
            foreach(Tuple<string, double> language_pair in languages) {
                requestMessage.Headers.AcceptLanguage.Add(
                    new StringWithQualityHeaderValue(language_pair.Item1, language_pair.Item2));
            }
        }
        public static void BatchAddHeaderAcceptEncoding(
            HttpRequestMessage requestMessage,
            string[] encodings
        ) {
            foreach(string encoding in encodings) {
                requestMessage.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(encoding));
            }
        }
        public static async Task<(bool, byte[])> RequestData(
            string url,
            string? referrer = null,
            int retryTime = 3
        ) {
            if (retryTime <= 0) {
                return (false, []);
            }
            try {
                HttpRequestMessage requestMessage = new() {
                    RequestUri = new(url),
                };
                // * Init the Header.
                BatchAddHeaderAcceptLanguage(requestMessage, [
                    new("zh-CN", 1.0),
                    new("zh", 0.9),
                    new("en-US", 0.8),
                    new("en", 0.7),
                ]);
                BatchAddHeaderAcceptEncoding(requestMessage, [
                    "gzip",
                    "deflate",
                    "br"
                ]);

                if (referrer != null) {
                    requestMessage.Headers.Referrer = new Uri(referrer);
                }
                byte[] dataResult = [];
                HttpClient httpClient = HttpClientFactory.GetHttpClient();
                try {
                    TryToAddCookie(requestMessage);
                } catch (Exception e) {
                    CoreManager.logger.Error(e);
                }
                using HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();
                string? contentEncoding = response.Content.Headers.ContentEncoding.ToString();

                try {
                    StringBuilder stringBuilder = new();
                    var cookies = response.Headers.GetValues("set-cookie");
                    foreach (var cookie in cookies) {
                        stringBuilder.AppendLine(cookie);
                    }
                    CoreManager.cookieMgr.UpdateCookies(stringBuilder.ToString());
                    CoreManager.cookieMgr.TryToUpdateCookies();
                } catch (Exception) { }

                if (contentEncoding != null) {
                    switch (CheckEncodingType(contentEncoding)) {
                        case ENCODE_TYPE.GZIP:
                            using (GZipStream stream = new(response.Content.ReadAsStream(), CompressionMode.Decompress)) {
                                using MemoryStream ms = new();
                                byte[] buf = new byte[1024];
                                int currentPointer = 0;
                                while ((currentPointer = stream.Read(buf, 0, buf.Length)) != 0) {
                                    ms.Write(buf, 0, currentPointer);
                                }
                                ms.Seek(0, SeekOrigin.Begin);
                                dataResult = ms.ToArray();
                                Console.WriteLine("GZIP Decompress complete.");
                            }
                            break;
                        case ENCODE_TYPE.DEFLATE:
                            using (DeflateStream stream = new(response.Content.ReadAsStream(), CompressionMode.Decompress)) {
                                using MemoryStream ms = new();
                                byte[] buf = new byte[1024];
                                int currentPointer = 0;
                                while ((currentPointer = stream.Read(buf, 0, buf.Length)) != 0) {
                                    ms.Write(buf, 0, currentPointer);
                                }
                                ms.Seek(0, SeekOrigin.Begin);
                                dataResult = ms.ToArray();
                                Console.WriteLine("DEFLATE Decompress complete.");
                            }
                            break;
                        case ENCODE_TYPE.BR:
                            using (BrotliStream stream = new(response.Content.ReadAsStream(), CompressionMode.Decompress)) {
                                using MemoryStream ms = new();
                                byte[] buf = new byte[1024];
                                int currentPointer = 0;
                                while ((currentPointer = stream.Read(buf, 0, buf.Length)) != 0) {
                                    ms.Write(buf, 0, currentPointer);
                                }
                                ms.Seek(0, SeekOrigin.Begin);
                                dataResult = ms.ToArray();
                                Console.WriteLine("BR Decompress complete");
                            }
                            break;
                        default:
                            using (Stream stream = response.Content.ReadAsStream()) {
                                using MemoryStream ms = new();
                                byte[] buf = new byte[1024];
                                int currentPointer = 0;
                                while ((currentPointer = stream.Read(buf, 0, buf.Length)) != 0) {
                                    ms.Write(buf, 0, currentPointer);
                                }
                                ms.Seek(0, SeekOrigin.Begin);
                                dataResult = ms.ToArray();
                                Console.WriteLine("Normal Read complete");
                            }
                            break;
                    }
                    return (true, dataResult);
                } else {
                    return (false, []);
                }
            } catch (WebException e) {
                Console.WriteLine("RequestWeb()发生Web异常: {0}", e);
                CoreManager.logger.Error(e);
                return await RequestData(url, referrer, retryTime - 1);
            } catch (IOException e) {
                Console.WriteLine("RequestWeb()发生IO异常: {0}", e);
                CoreManager.logger.Error(e);
                return await RequestData(url, referrer, retryTime - 1);
            } catch (Exception e) {
                Console.WriteLine("RequestWeb()发生其他异常: {0}", e);
                CoreManager.logger.Error(e);
                return await RequestData(url, referrer, retryTime - 1);
            }
        }
        public static async Task<(bool, string)> RequestJson(
            string url,
            string methodName,
            bool useWbi = false,
            Dictionary<string, string>? parameters = null,
            string referrer = "https://www.bilibili.com/",
            int retryTime = 3
        ) {
            if (retryTime <= 0) {
                return (false, "The request exceeded the retry limit.");
            }
            StringBuilder urlBuilder = new(url);

            // * Append the parameters to the url.
            if (parameters != null && parameters.Count != 0) {
                urlBuilder.Append('?');
                string paramStr;
                if (useWbi) {
                    paramStr = await WbiAPI.GetAppendWbiUrl(parameters);
                } else {
                    paramStr = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();
                }
                urlBuilder.Append(paramStr);
            }

            try {
                HttpRequestMessage requestMessage = new() {
                    RequestUri = new(urlBuilder.ToString()),
                };
                // * Select the Http Method.
                if (methodName.Equals("get", StringComparison.CurrentCultureIgnoreCase)) {
                    requestMessage.Method = HttpMethod.Get;
                } else if (methodName.Equals("post", StringComparison.CurrentCultureIgnoreCase)) {
                    requestMessage.Method = HttpMethod.Post;
                }
                // * Init the Header.
                BatchAddHeaderAcceptLanguage(requestMessage, [
                    new("zh-CN", 1.0),
                    new("zh", 0.9),
                    new("en-US", 0.8),
                    new("en",0.7),
                ]);
                BatchAddHeaderAcceptEncoding(requestMessage, [
                    "gzip","deflate","br"
                ]);

                if (referrer != null) {
                    requestMessage.Headers.Referrer = new Uri(referrer);
                }

                string jsonResult = string.Empty;
                HttpClient httpClient = HttpClientFactory.GetHttpClient();
                try {
                    TryToAddCookie(requestMessage);
                } catch (Exception e) {
                    CoreManager.logger.Error(e);
                }
                using HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();
                string? contentEncoding = response.Content.Headers.ContentEncoding.ToString();
                
                try {
                    StringBuilder stringBuilder = new();
                    var cookies = response.Headers.GetValues("set-cookie");
                    foreach(var cookie in cookies) {
                        stringBuilder.AppendLine(cookie);
                    }
                    CoreManager.cookieMgr.UpdateCookies(stringBuilder.ToString());
                    CoreManager.cookieMgr.TryToUpdateCookies();
                } catch (Exception) {}
                
                if (contentEncoding != null) {
                    switch(CheckEncodingType(contentEncoding)) {
                        case ENCODE_TYPE.GZIP:
                            using (GZipStream stream = new(response.Content.ReadAsStream(), CompressionMode.Decompress)) {
                                using StreamReader streamReader = new(stream, Encoding.UTF8);
                                jsonResult = streamReader.ReadToEnd();
                                Console.WriteLine("GZIP Decompress complete.");
                            }
                            break;
                        case ENCODE_TYPE.DEFLATE:
                            using  (DeflateStream stream = new(response.Content.ReadAsStream(), CompressionMode.Decompress)) {
                                using StreamReader streamReader = new(stream, Encoding.UTF8);
                                jsonResult = streamReader.ReadToEnd();
                                Console.WriteLine("DEFLATE Decompress complete.");
                            }
                            break;
                        case ENCODE_TYPE.BR:
                            using (BrotliStream stream = new(response.Content.ReadAsStream(), CompressionMode.Decompress)) {
                                using StreamReader streamReader = new(stream, Encoding.UTF8);
                                jsonResult = streamReader.ReadToEnd();
                                Console.WriteLine("BR Decompress complete");
                            }
                            break;
                        default:
                            using (Stream stream = response.Content.ReadAsStream()) {
                                using StreamReader streamReader = new(stream, Encoding.UTF8);
                                jsonResult = streamReader.ReadToEnd();
                                Console.WriteLine("Normal Read complete");
                            }
                            break;
                    }
                    return (true, jsonResult);
                } else {
                    return (false, "An error occurred in the transmission response.");
                }
            } catch (WebException e) {
                Console.WriteLine("RequestWeb()发生Web异常: {0}", e);
                CoreManager.logger.Error(e);
                return await RequestJson(urlBuilder.ToString(), methodName, useWbi, null, referrer, retryTime - 1);
            } catch (IOException e) {
                Console.WriteLine("RequestWeb()发生IO异常: {0}", e);
                CoreManager.logger.Error(e);
                return await RequestJson(urlBuilder.ToString(), methodName, useWbi, null, referrer, retryTime - 1);
            } catch (Exception e) {
                Console.WriteLine("RequestWeb()发生其他异常: {0}", e);
                CoreManager.logger.Error(e);
                return await RequestJson(urlBuilder.ToString(), methodName, useWbi, null, referrer, retryTime - 1);
            }
        }

        static void TryToAddCookie(HttpRequestMessage message) {
            if(!CoreManager.cookieMgr.CheckCookieFileExist()) { return; }
            if(message.RequestUri == null) { return; }
            var messageUri = message.RequestUri;
            foreach(Cookie cookie in CoreManager.cookieMgr.TryToGetCookies().Cast<Cookie>()) {
                if(!messageUri.Host.Contains(cookie.Domain)) { continue; }
                if(cookie.Path == @"/" || messageUri.AbsolutePath.Contains(cookie.Path)) {
                    if(cookie.Expires < DateTime.UtcNow) { continue; }
                    if(cookie.Secure && messageUri.Scheme != "https") { continue; }
                    if(cookie.HttpOnly && messageUri.Scheme != "https" && messageUri.Scheme != "http") { continue; }
                    message.Headers.Add("Cookie",string.Format("{0}={1}", cookie.Name, cookie.Value));
                }
            }
        }
    }
}