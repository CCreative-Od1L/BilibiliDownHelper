using System.IO;
using System.Net;
using System.Text;
using System.Net.Http.Headers;
using System.IO.Compression;

namespace Utils {
    enum ENCODE_TYPE{
        DEFAULT,
        GZIP,
        DEFLATE,
        BR,
    };

    internal static class WebClient {
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
        public static async Task<Tuple<bool, string>> Request(
            string url,
            string methodName,
            Dictionary<string, string>? parameters = null,
            string? referrer = null,
            int retryTime = 3
        ) {
            if (retryTime <= 0) {
                return new Tuple<bool, string>(true, string.Empty);
            }

            if (parameters != null && parameters.Count != 0) {
                StringBuilder stringBuilder = new("?");
                foreach (var param in parameters) {
                    stringBuilder.AppendFormat("{0}={1}&", param.Key, param.Value);
                }
                stringBuilder.Length--;  // * Remove the last "&".
                url += stringBuilder.ToString();
            }

            // GET Method => use GetAsync
            // POST Method => use PostAsync
            try {
                HttpRequestMessage requestMessage = new()
                {
                    RequestUri = new(url),
                };
                if (methodName.ToLower() == "get") {
                    requestMessage.Method = HttpMethod.Get;
                } else if (methodName.ToLower() == "post") {
                    requestMessage.Method = HttpMethod.Post;
                }

                BatchAddHeaderAcceptLanguage(requestMessage, new Tuple<string, double>[]{
                    new("zh-CN", 1.0),
                    new("zh", 0.9),
                    new("en-US", 0.8),
                    new("en",0.7),
                });
                BatchAddHeaderAcceptEncoding(requestMessage, new string[]{
                    "gzip","deflate","br"
                });

                if (referrer != null) {
                    requestMessage.Headers.Referrer = new Uri(referrer);
                }

                // cookie添加
                if(!url.Contains("getLogin")) {

                }

                string jsonResult = string.Empty;
                using HttpResponseMessage response = await HttpClientFactory.GetHttpClient().SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();
                string? contentEncoding = response.Content.Headers.ContentEncoding.ToString();
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
                    return new Tuple<bool, string>(true, jsonResult);
                } else {
                    return new Tuple<bool, string>(false, string.Empty);
                }
            } catch (WebException e) {
                Console.WriteLine("RequestWeb()发生Web异常: {0}", e);
                // Logging.LogManager.Error(e); // * 日志记录
                return Request(url, methodName, parameters, referrer, retryTime - 1).Result;
            } catch (IOException e) {
                Console.WriteLine("RequestWeb()发生IO异常: {0}", e);
                // Logging.LogManager.Error(e);
                return Request(url, methodName, parameters, referrer, retryTime - 1).Result;
            } catch (Exception e) {
                Console.WriteLine("RequestWeb()发生其他异常: {0}", e);
                // Logging.LogManager.Error(e);
                return Request(url, methodName, parameters, referrer, retryTime - 1).Result;
            }
        }
    }
}