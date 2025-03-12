namespace PoemTown.Repository.Utils;

public static class OrderCodeGenerator
{
    public static string Generate(int length = 9)
    {
        return DateTimeHelper.SystemTimeNow.ToString("yyMMdd") + "-" + "PT" + StringHelper.GenerateRandomString(length);
    }
}