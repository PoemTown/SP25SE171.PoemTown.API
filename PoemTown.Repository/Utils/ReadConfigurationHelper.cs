using Microsoft.Extensions.Configuration;

namespace PoemTown.Repository.Utils;

public static class ReadConfigurationHelper
{
    /// <summary>
    /// Read the appsettings.Development.json file from the app directory in development phase.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public static IConfiguration ReadDevelopmentAppSettings()
    {
        IConfiguration configuration = null;
        var pathDocker = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../app"));
        var pathLocal = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../PoemTown.API"));

        try
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(pathDocker)
                .AddJsonFile("appsettings.Development.json")
                .Build();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (configuration == null)
        {
            try
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(pathLocal)
                    .AddJsonFile("appsettings.Development.json")
                    .Build();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new FileNotFoundException("Unable to load configuration from any path.");
            }
        }
        return configuration;
    }
    
    /// <summary>
    /// Retrieves an environment variable.
    /// </summary>
    /// <param name="key">The name of the environment variable.</param>
    /// <returns>Environment variable value.</returns>
    public static string GetEnvironmentVariable(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return "";
        }
        return Environment.GetEnvironmentVariable(key) ?? "";
    }
    
    /*public static string GetModelOnnxPath()
    {
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        
        // If not in development, return the path to the model in the app directory
        if (!isDevelopment)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "app", "PlagiarismDetector", "HuggingFace", "model.onnx");
        }
        
        // If in development, return the path to the model in the PoemTown.Service directory
        var pathDocker = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../src/PoemTown.Service"));
        var pathLocal = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../PoemTown.Service"));

        var modelPath = Path.Combine(pathDocker, "PlagiarismDetector", "HuggingFace", "model.onnx");
        
        // If the model path does not exist in the Docker directory, return the path in the local directory
        if (!File.Exists(modelPath))
        {
            modelPath = Path.Combine(pathLocal, "PlagiarismDetector", "HuggingFace", "model.onnx");
        }
        return modelPath;
    }*/
}