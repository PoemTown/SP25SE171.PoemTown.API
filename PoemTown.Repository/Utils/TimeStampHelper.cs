using System.Security.Cryptography.X509Certificates;

namespace PoemTown.Repository.Utils;

public static class TimeStampHelper
{
    /// <summary>
    /// Generate TimeStamp as UnixTimeStamp format at the moment
    /// </summary>
    /// <param name="isMilliSecond">Return as MilliSeconds format if true</param>
    /// <returns>MilliSeconds if true otherwise Seconds</returns>
    public static long GenerateUnixTimeStampNow(bool isMilliSecond = false)
    {
        return isMilliSecond
            ? DateTimeHelper.SystemTimeNow.ToUnixTimeMilliseconds()
            : DateTimeHelper.SystemTimeNow.ToUnixTimeSeconds();
    }
    
    /// <summary>
    /// Generate TimeStamp as TimeStamp format at the moment
    /// </summary>
    /// <param name="isMilliSecond">Return as MilliSeconds format if true</param>
    /// <returns>MilliSeconds "yyyyMMddHHmmssfff" if true otherwise Seconds "yyyyMMddHHmmss"</returns>
    public static long GenerateTimeStampNow(bool isMilliSecond = false)
    {
        return isMilliSecond
            ? long.Parse(DateTimeHelper.SystemTimeNow.ToString("yyyyMMddHHmmssfff"))
            : long.Parse(DateTimeHelper.SystemTimeNow.ToString("yyyyMMddHHmmss"));
    }
    
    /// <summary>
    /// Generate TimeStamp as UnixTimeStamp format with specific hours, minutes, seconds
    /// </summary>
    /// <param name="hours">Hours</param>
    /// <param name="minutes">Minutes</param>
    /// <param name="seconds">Seconds</param>
    /// <param name="isMilliSecond">Return as MilliSeconds format if true</param>
    /// <returns>MilliSeconds if true otherwise Seconds"</returns>
    public static long GenerateUnixTimeStamp(int? hours, int? minutes, int? seconds, bool isMilliSecond = false)
    {
        DateTimeOffset unixTimeStamp = DateTimeHelper.SystemTimeNow
            .AddHours(hours ?? 0)
            .AddMinutes(minutes ?? 0)
            .AddSeconds(seconds ?? 0);
        
        return isMilliSecond
            ? unixTimeStamp.ToUnixTimeMilliseconds()
            : unixTimeStamp.ToUnixTimeSeconds();
    }
    
    /// <summary>
    /// Generate TimeStamp as TimeStamp format with specific hours, minutes, seconds
    /// </summary>
    /// <param name="hours">Hours</param>
    /// <param name="minutes">Minutes</param>
    /// <param name="seconds">Seconds</param>
    /// <param name="isMilliSecond">Return as MilliSeconds format if true</param>
    /// <returns>MilliSeconds "yyyyMMddHHmmssfff" if true otherwise Seconds "yyyyMMddHHmmss""</returns>
    public static long GenerateTimeStamp(int? hours, int? minutes, int? seconds, bool isMilliSecond = false)
    {
        var timeStamp = DateTimeHelper.SystemTimeNow
            .AddHours(hours ?? 0)
            .AddMinutes(minutes ?? 0)
            .AddSeconds(seconds ?? 0);
        
        return isMilliSecond
            ? long.Parse(timeStamp.ToString("yyyyMMddHHmmssfff"))
            : long.Parse(timeStamp.ToString("yyyyMMddHHmmss"));
    }
}