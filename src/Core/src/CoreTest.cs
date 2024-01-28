
namespace Core {
    public static class CoreTest {
        public static async Task TestAsync() {
            CoreManager.logger.Info("Main", "Logger start");
            Console.WriteLine("System start");

            string url = "https://api.bilibili.com/x/web-interface/popular/precious";
            string methodName = "get";

            Console.WriteLine("Web Request Start");
            var result = await Web.WebClient.Request(url: url, methodName: methodName);
            if (result.Item1 == false) {
                Console.WriteLine(result.Item2);
            } else {
                Console.WriteLine(result.Item2);
            }

            Thread.Sleep(5000);

            Console.WriteLine("System End");
        }
    }
}