using Core.Logger;
LogManager.Info("Main", "Logger start");
Console.WriteLine("System start");

string url = "http://httpbin.org/get";
string methodName = "get";
Dictionary<string, string> parameters = new()
{
    { "c_id", "114514" },
    { "p_id", "1919810" },
    { "a_id", "170001" },
};

Console.WriteLine("Web Request Start");
var result = await Core.Web.WebClient.Request(url, methodName, parameters);
if (result.Item1 == false) {
    Console.WriteLine(result.Item2);
} else {
    Console.WriteLine(result.Item2);
}

Thread.Sleep(1000);