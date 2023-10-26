
string url = "http://httpbin.org/get";
string methodName = "Get";
string Result;
Dictionary<string, string> parameters = new()
{
    { "c_id", "114514" },
    { "p_id", "1919810" },
    { "a_id", "170001" },
};

Console.WriteLine("Web Request Start");
Utils.WebClient.Request(url, methodName, out Result, parameters);
Console.WriteLine(Result);