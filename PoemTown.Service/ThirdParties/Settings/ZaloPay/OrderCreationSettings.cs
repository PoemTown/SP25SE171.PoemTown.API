using PoemTown.Repository.Utils;

namespace PoemTown.Service.ThirdParties.Settings.ZaloPay;

public class OrderCreationSettings<TItem>
{
    public string AppUser { get; set; }
    public string ApptransId { get; set; }
    public long AppTime { get; set; } = TimeStampHelper.GenerateUnixTimeStampNow(true);
    public long? ExpireDurationSeconds { get; set; } = 10000;
    public long Amount { get; set; }
    public IList<TItem> Items { get; set; }
    public string Description { get; set; }
    public string EmbedData { get; set; }
    public string BankCode { get; set; }
    public string Mac { get; set; }
}