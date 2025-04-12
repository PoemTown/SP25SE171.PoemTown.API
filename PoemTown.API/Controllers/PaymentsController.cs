using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.ConfigurationModels.Payment;
using PoemTown.Service.BusinessModels.RequestModels.PaymentRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.Interfaces;
using PoemTown.Service.ThirdParties.Settings.ZaloPay;
using PoemTown.Service.ThirdParties.Utils.Zalopay;

namespace PoemTown.API.Controllers;

public class PaymentsController : BaseController
{
    private readonly IPaymentService _paymentService;
    private readonly PaymentRedirectSettings _paymentRedirectSettings;
    private readonly ZaloPaySettings _zaloPaySettings;

    public PaymentsController(ZaloPaySettings zaloPaySettings,
        IPaymentService paymentService,
        PaymentRedirectSettings paymentRedirectSettings)
    {
        _zaloPaySettings = zaloPaySettings;
        _paymentService = paymentService;
        _paymentRedirectSettings = paymentRedirectSettings;
    }

    [HttpGet]
    [Route("v1/zalopay/callback")]
    public async Task<RedirectResult> ZaloPayCallbackAsync(ZaloPayCallBackRequest request)
    {
        string redirectUrl = _paymentRedirectSettings.RedirectFailureUrl;
        try
        {
            switch (request.Status)
            {
                case 1:
                {
                    var handleCallbackPaymentSuccessRequest = new HandleCallbackPaymentRequest()
                        // Handle payment success
                        {
                            OrderCode = request.ApptransId,
                            BankCode = request.BankCode,
                            AppId = request.AppId.ToString(),
                            Amount = request.Amount,
                            DiscountAmount = request.DiscountAmount,
                            Checksum = request.Checksum,
                            Status = request.Status
                        };
                    await _paymentService.HandleCallbackPaymentSuccessAsync(handleCallbackPaymentSuccessRequest);
                    redirectUrl = _paymentRedirectSettings.RedirectSuccessUrl;
                    break;
                }
                default:
                    // Handle payment failure
                    var handleCallbackPaymentCancelledRequest = new HandleCallbackPaymentRequest
                    {
                        OrderCode = request.ApptransId,
                    };
                    await _paymentService.HandleCallbackPaymentCancelledAsync(handleCallbackPaymentCancelledRequest);
                    redirectUrl = _paymentRedirectSettings.RedirectFailureUrl;
                    break;
            }
        }
        catch
        {
            return Redirect(_paymentRedirectSettings.RedirectFailureUrl);
        }

        return Redirect(redirectUrl);
    }
    
    [HttpGet]
    [Route("v1/vnpay/callback")]
    public async Task<RedirectResult> VnPayCallbackAsync(VnPayCallBackRequest request)
    {
        string redirectUrl = _paymentRedirectSettings.RedirectFailureUrl;
        try
        {
            switch (request.VnpTransactionStatus)
            {
                case 00:
                {
                    var handleCallbackPaymentSuccessRequest = new HandleCallbackPaymentRequest()
                        // Handle payment success
                        {
                            OrderCode = request.VnpTxnRef,
                            BankCode = request.VnpBankCode,
                            AppId = request.VnpTmnCode,
                            Amount = request.VnpAmount / 100,
                            DiscountAmount = 0,
                            Checksum = request.VnpSecureHash,
                            Status = 1
                        };
                    await _paymentService.HandleCallbackPaymentSuccessAsync(handleCallbackPaymentSuccessRequest);
                    redirectUrl = _paymentRedirectSettings.RedirectSuccessUrl;
                    break;
                }
                default:
                    // Handle payment failure
                    var handleCallbackPaymentCancelledRequest = new HandleCallbackPaymentRequest
                    {
                        OrderCode = request.VnpTxnRef,
                    };
                    await _paymentService.HandleCallbackPaymentCancelledAsync(handleCallbackPaymentCancelledRequest);
                    redirectUrl = _paymentRedirectSettings.RedirectFailureUrl;
                    break;
            }
        }
        catch
        {
            return Redirect(_paymentRedirectSettings.RedirectFailureUrl);
        }

        return Redirect(redirectUrl);
    }
}