using Core.Logger;
LogManager.Info("Main", "Logger start");
Console.WriteLine("System start");

// string url = "https://passport.bilibili.com/x/passport-login/web/qrcode/generate";
// string methodName = "get";

// Console.WriteLine("Web Request Start");
// var result = await Core.Web.WebClient.Request(url, methodName);
// if (result.Item1 == false) {
//     Console.WriteLine(result.Item2);
// } else {
//     Console.WriteLine(result.Item2);
// }

Thread.Sleep(5000);

Console.WriteLine("System End");