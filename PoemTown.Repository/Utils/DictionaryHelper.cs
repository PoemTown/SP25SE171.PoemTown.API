namespace PoemTown.Repository.Utils;

public static class DictionaryHelper
{
    public static T GetValueOrDefault<T>(this IDictionary<string, object> dictionary, string key, T defaultValue)
    {
        if (dictionary.TryGetValue(key, out var value) && value != null)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        return defaultValue;
    }
}