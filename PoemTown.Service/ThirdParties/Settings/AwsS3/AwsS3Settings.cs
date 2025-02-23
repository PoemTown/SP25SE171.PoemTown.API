namespace PoemTown.Service.ThirdParties.Settings.AwsS3;

public class AwsS3Settings
{
    public string ServiceUrl { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string BucketName { get; set; }
    
    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(ServiceUrl))
        {
            throw new ArgumentNullException("ServiceUrl cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(AccessKey))
        {
            throw new ArgumentNullException("AccessKey cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(SecretKey))
        {
            throw new ArgumentNullException("SecretKey cannot be null or empty");
        }

        if (string.IsNullOrEmpty(BucketName))
        {
            throw new ArgumentNullException("BucketName cannot be null or empty");
        }

        return true;
    }
}