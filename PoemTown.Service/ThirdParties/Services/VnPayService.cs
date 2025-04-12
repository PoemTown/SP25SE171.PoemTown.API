using System.Globalization;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Transactions;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Settings.VnPay;
using PoemTown.Service.ThirdParties.Utils.VnPay;

namespace PoemTown.Service.ThirdParties.Services;

public class VnPayService : IVnPayService, IPaymentMethod
{
    private readonly VnPaySettings _vnPaySettings;
    private readonly IUnitOfWork _unitOfWork;

    public VnPayService(VnPaySettings vnPaySettings, 
        IUnitOfWork unitOfWork)
    {
        _vnPaySettings = vnPaySettings;
        _unitOfWork = unitOfWork;
    }

    public string CreatePaymentUrl(VnPayOrderCreationSettings order)
    {
        //Vnpay config
        string vnpReturnUrl = _vnPaySettings.Vnp_ReturnUrl;
        string vnpUrl = _vnPaySettings.Vnp_Url;
        string vnpTmnCode = _vnPaySettings.Vnp_TmnCode;
        string vnpHashSecret = _vnPaySettings.Vnp_HashSecret;

        VnPayLibrary vnpay = new VnPayLibrary();

        //Get TimeStamp
        string vnpCreateDate = TimeStampHelper.GenerateTimeStampNow().ToString();
        string vnpExpireDate = TimeStampHelper.GenerateTimeStamp(0, 15, 0).ToString();

        //Add Vnpay data to query string
        vnpay.AddRequestData("vnp_Version", "2.1.0");
        vnpay.AddRequestData("vnp_Command", "pay");
        vnpay.AddRequestData("vnp_TmnCode", vnpTmnCode);
        vnpay.AddRequestData("vnp_Locale", "vn");
        vnpay.AddRequestData("vnp_CurrCode", "VND");
        vnpay.AddRequestData("vnp_TxnRef", order.TransactionCode); //orderId (appointmentId)
        vnpay.AddRequestData("vnp_OrderInfo", order.TransactionDescription); //orderDescription
        vnpay.AddRequestData("vnp_OrderType", "other"); //orderType (nay dua theo mat hang vnpay quy dinh)
        vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString(CultureInfo.InvariantCulture));
        vnpay.AddRequestData("vnp_ReturnUrl", vnpReturnUrl); //Return url de get param tu vnpay
        vnpay.AddRequestData("vnp_ExpireDate", vnpExpireDate); //Set thoi gian het han cua don hang (o day tinh la 5p)
        vnpay.AddRequestData("vnp_IpAddr", order.UserIpAddress); //Ip cua user thanh toan
        vnpay.AddRequestData("vnp_CreateDate", vnpCreateDate);

        //Process Vnpay with Vnpay Library
        string paymentUrl = vnpay.CreateRequestUrl(vnpUrl, vnpHashSecret);
        return paymentUrl;
    }

    public async Task<DepositUserEWalletResponse> DepositUserEWalletPayment(UserEWalletData request)
    {
        string transactionCode = OrderCodeGenerator.Generate();
        
        // Create order settings
        VnPayOrderCreationSettings orderCreationSettings = new VnPayOrderCreationSettings()
        {
            TransactionCode = transactionCode,
            Amount = request.Amount,
            TransactionDescription = $"Nap {request.Amount} vao vi dien tu",
            UserIpAddress = request.UserIpAddress ?? "",
        };

        string paymentUrl = CreatePaymentUrl(orderCreationSettings);
        
        Transaction transaction = new Transaction()
        {
            UserEWalletId = request.UserEWalletId,
            Amount = request.Amount,
            Description = orderCreationSettings.TransactionDescription,
            TransactionCode = transactionCode,
            Type = TransactionType.EWalletDeposit,
            Status = TransactionStatus.Pending,
            PaymentGateway = await _unitOfWork.GetRepository<PaymentGateway>().FindAsync(p => p.Name == "Vnpay"),
        };

        await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
        
        // Create response
        DepositUserEWalletResponse response = new DepositUserEWalletResponse()
        {
            Code = 1,
            Message = "Create payment url successfully",
            IsSuccess = true,
            PaymentUrl = paymentUrl,
            OrderCode = transactionCode,
        };
        return response;
    }
}