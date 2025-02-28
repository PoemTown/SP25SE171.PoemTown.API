namespace PoemTown.Service.BusinessModels.ConfigurationModels.Payment;

public class PaymentRedirectSettings
{
    public string RedirectSuccessUrl { get; set; }
    public string RedirectFailureUrl { get; set; }
    
    public bool IsValid()
    {
        if (String.IsNullOrWhiteSpace(RedirectSuccessUrl))
        {
            throw new ArgumentNullException("RedirectSuccessUrl is required");
        }
        if (String.IsNullOrWhiteSpace(RedirectFailureUrl))
        {
            throw new ArgumentNullException("RedirectFailureUrl is required");
        }
        return true;
    }
    
}