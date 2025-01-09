namespace PoemTown.Repository.Utils;

public static class StringHelper
{
    public static string NullOrWhiteSpaceStringHandle(string inputString)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            throw new ArgumentException("String is null or white space");
        }

        return inputString;
    }
    
    /// <summary>
    /// Uppercase the string and trim whitespace
    /// </summary>
    /// <param name="inputString">The string needed to format</param>
    /// <returns></returns>
    public static string CapitalizeString(string inputString)
    {
        NullOrWhiteSpaceStringHandle(inputString);
        return inputString.Trim().ToUpper();
    }

    /// <summary>
    /// Normalize string and trim whitespace
    /// </summary>
    /// <param name="inputString">The string needed to format</param>
    /// <returns></returns>
    public static string NormalizeString(string inputString)
    {
        NullOrWhiteSpaceStringHandle(inputString);
        return inputString.Trim().ToLower();
    }
    
    /// <summary>
    /// Reformat string:
    /// - Uppercase first letter
    /// - Uppercase the letter after whitespace
    /// - Remove whitespace at the beginning and end of the string
    /// - The rest of the string is lowercase
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    public static string FormatStringInput(string inputString)
    {
        NullOrWhiteSpaceStringHandle(inputString); // Ensure this method checks for null or whitespace

        string[] words = inputString.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries); // Split input into words
        string result = "";

        foreach (var word in words)
        {
            var normalizedString = NormalizeString(word); // Assuming this normalizes the string
            // Capitalize the first letter and make the rest lowercase
            string capitalizedString = CapitalizeFirstLetters(normalizedString);
            result += capitalizedString + " "; // Append to result
        }

        return result.Trim(); // Trim any trailing spaces
    }

    public static string CapitalizeFirstLetters(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Capitalize the first letter and make the rest lowercase
        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }
}