using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Orders;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Models.ZaloPay;
using PoemTown.Service.ThirdParties.Settings.ZaloPay;
using PoemTown.Service.ThirdParties.Utils.Zalopay;

namespace PoemTown.Service.ThirdParties.Services;

public class ZaloPayService : IZaloPayService, IPaymentMethod
{
    private readonly ZaloPaySettings _zaloPaySettings;
    private readonly IUnitOfWork _unitOfWork;
    private const string ZalopayCreateOrderUrl = "https://sandbox.zalopay.com.vn/v001/tpe/createorder";
    public ZaloPayService(ZaloPaySettings zaloPaySettings, IUnitOfWork unitOfWork)
    {
        _zaloPaySettings = zaloPaySettings;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ZaloPayResponseData> CreateOrderUrl<TItem>(OrderCreationSettings<TItem> orderCreationSettings)
    {
        var param = new Dictionary<string, string>();
        /*
        var embeddata = JsonConvert.SerializeObject(orderCreationSettings.Items);
        */
        var embeddata = JsonConvert.SerializeObject(new
        {
            redirecturl = _zaloPaySettings.CallbackUrl,
        });
        var appTransId = $"{orderCreationSettings.ApptransId}";
        param.Add("appid", _zaloPaySettings.AppId);
        param.Add("appuser", orderCreationSettings.AppUser);
        param.Add("apptime", orderCreationSettings.AppTime.ToString());
        param.Add("amount", orderCreationSettings.Amount.ToString());
        param.Add("apptransid", appTransId);
        param.Add("embeddata", embeddata);
        param.Add("item", JsonConvert.SerializeObject(orderCreationSettings.Items));
        param.Add("description", orderCreationSettings.Description);
        param.Add("bankcode", orderCreationSettings.BankCode);
        /*if (orderCreationSettings.UserPhone != null) param.Add("phone", orderCreationSettings.UserPhone);
        if (orderCreationSettings.UserEmail != null) param.Add("email", orderCreationSettings.UserEmail);*/

        var macData = _zaloPaySettings.AppId + "|" + param["apptransid"] + "|" + param["appuser"] + "|" +
                      param["amount"] + "|" + param["apptime"] + "|"
                      + param["embeddata"] + "|" + param["item"];
        
        var mac = ZaloPayHmacHelper.HmacHelper.Compute(ZaloPayHmacHelper.ZaloPayHMAC.HMACSHA256, _zaloPaySettings.Key1, macData);
        
        param.Add("mac", mac);

        var result = await HttpHelper.PostFormAsync(ZalopayCreateOrderUrl, param);

        return new ZaloPayResponseData()
        {
            OrderCode = appTransId,
            SubReturnCode = result.GetValueOrDefault("subreturncode", -1),
            SubReturnMessage = result.GetValueOrDefault("subreturnmessage", ""),
            OrderUrl = result.GetValueOrDefault("orderurl", ""),
            ZpTransToken = result.GetValueOrDefault("zptranstoken", ""),
            OrderToken = result.GetValueOrDefault("ordertoken", ""),
            QrCode = result.GetValueOrDefault("qrCode", ""),
            ReturnMessage = result.GetValueOrDefault("returnmessage", ""),
            ReturnCode = result.GetValueOrDefault("returncode", -1),
            Amount = result.GetValueOrDefault("amount", 0),
            AppId = _zaloPaySettings.AppId,
        };
    }

    public async Task<DepositUserEWalletResponse> DepositUserEWalletPayment(UserEWalletData request)
    {
        // Create order
        var orderCreationSettings = new OrderCreationSettings<UserEWalletData>()
        {
            AppUser = request.UserId.ToString(),
            ApptransId = OrderCodeGenerator.Generate(),
            Amount = (long)request.Amount,
            Description = $"Nạp: {request.Amount}VNĐ vào ví điện tử",
            Items = new List<UserEWalletData>
            {
                new()
                {
                    Amount = request.Amount,
                    UserId = request.UserId,
                    UserEWalletId = request.UserEWalletId
                }
            },
            BankCode = "",
        };

        var response = await CreateOrderUrl(orderCreationSettings);
        
        // Return when deposit failed
        if (response.ReturnCode != 1)
        {
            return new DepositUserEWalletResponse()
            {
                Message = response.ReturnMessage ?? "Deposit EWallet failed",
                Code = response.ReturnCode ?? -1,
                IsSuccess = false
            };
        }

        // Create order
        Order order = new Order()
        {
            UserId = request.UserId,
            Type = OrderType.EWalletDeposit,
            Amount = request.Amount,
            OrderDate = DateTimeOffset.Now,
            OrderDescription = orderCreationSettings.Description,
            OrderCode = response.OrderCode,
            OrderToken = response.ZpTransToken,
            Status = OrderStatus.Pending,
            PaymentGateway = await _unitOfWork.GetRepository<PaymentGateway>().FindAsync(p => p.Name == "Zalopay")
        };
        await _unitOfWork.GetRepository<Order>().InsertAsync(order);
        
        // Create order details
        IList<OrderDetail> orderDetails = new List<OrderDetail>();
        foreach (var item in orderCreationSettings.Items)
        {
            orderDetails.Add(new OrderDetail()
            {
                Order = order,
                ItemPrice = item.Amount,
                ItemQuantity = 1,
                UserEWalletId = item.UserEWalletId
            });
        }
        await _unitOfWork.GetRepository<OrderDetail>().InsertRangeAsync(orderDetails);

        await _unitOfWork.SaveChangesAsync();
        
        // Return when deposit failed
        return new DepositUserEWalletResponse()
        {
            Message = "Deposit EWallet success",
            PaymentUrl = response.OrderUrl.ToString(),
            Token = response.ZpTransToken,
            Code = (int)response.ReturnCode!,
            IsSuccess = true,
            OrderCode = response.OrderCode,
        };
    }
}