namespace Core.FileFunc;
public class DataForm(string name, string content, bool enableCrypt)
{
    public string Name { get; set; } = name;
    public string Content { get; set; } = content;
    public bool EnableCrypt { get; set; } = enableCrypt;
}