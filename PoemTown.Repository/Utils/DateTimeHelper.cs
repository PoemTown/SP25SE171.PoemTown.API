namespace PoemTown.Repository.Utils;

public static class DateTimeHelper
{
    /// <summary>
    /// Return System Time Now in UTC+7
    /// </summary>
    public static DateTimeOffset SystemTimeNow => ConvertToUtcPlus7(DateTimeOffset.Now);
    
    /// <summary>
    /// Convert DateTimeOffset to UTC+7
    /// </summary>
    /// <param name="dateTime">DateTimeOffSet</param>
    /// <returns>DateTimeOffSet UTC+7</returns>
    public static DateTimeOffset ConvertToUtcPlus7(DateTimeOffset dateTime)
    {
        return dateTime.ToOffset(new TimeSpan(7, 0, 0));
    }
}