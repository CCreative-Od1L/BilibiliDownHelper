using System.Globalization;

namespace Core.Utils;

static public class DateTimeUtils {
    static public string GetCurrentTimestampSecond() {
        return Convert.ToString(
            Convert.ToInt64(
                (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds
            )
        );
    }
    public static DateTime TimestampToDateTime(string timestampStr) {
        var timestamp = long.Parse(timestampStr);
        string currentTimestamp = GetCurrentTimestampSecond();
        if (timestampStr.Length > currentTimestamp.Length) {
            return TimestampToDateTime(timestamp, true);
        } else {
            return TimestampToDateTime(timestamp);
        }
    }
    static DateTime TimestampToDateTime(long timestampVal, bool isMilliseconds = false) {
        return 
        isMilliseconds ? 
        DateTimeOffset.FromUnixTimeMilliseconds(timestampVal).DateTime.ToLocalTime() :
        DateTimeOffset.FromUnixTimeSeconds(timestampVal).DateTime.ToLocalTime();
    }
    public static DateTime TimeStringToDateTime(string timeString, string format) {
        // * 默认用的 米国 日期文字信息
        return DateTime.ParseExact(
            timeString,
            format,
            CultureInfo.GetCultureInfo("en-us"));
    }
}