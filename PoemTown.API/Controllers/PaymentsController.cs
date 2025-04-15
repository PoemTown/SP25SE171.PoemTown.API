using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.ConfigurationModels.Payment;
using PoemTown.Service.BusinessModels.RequestModels.PaymentRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.Interfaces;
using PoemTown.Service.ThirdParties.Settings.Stripe;
using PoemTown.Service.ThirdParties.Settings.ZaloPay;
using PoemTown.Service.ThirdParties.Utils.Zalopay;
using Stripe;
using Stripe.Checkout;

namespace PoemTown.API.Controllers;

public class PaymentsController : BaseController
{
    private readonly IPaymentService _paymentService;
    private readonly PaymentRedirectSettings _paymentRedirectSettings;
    private readonly ZaloPaySettings _zaloPaySettings;
    private readonly StripeSettings _stripeSettings;

    public PaymentsController(ZaloPaySettings zaloPaySettings,
        IPaymentService paymentService,
        PaymentRedirectSettings paymentRedirectSettings,
        StripeSettings stripeSettings)
    {
        _zaloPaySettings = zaloPaySettings;
        _paymentService = paymentService;
        _paymentRedirectSettings = paymentRedirectSettings;
        _stripeSettings = stripeSettings;
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

    [HttpPost]
    [Route("v1/stripe/callback")]
    public async Task<IActionResult> StripeCallbackAsync()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var secret = _stripeSettings.WebhookSecret;

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                secret
            );

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                if (session?.Metadata != null && session.Metadata.TryGetValue("order_code", out var orderCode))
                {
                    var handleCallbackPaymentSuccessRequest = new HandleCallbackPaymentRequest
                    {
                        OrderCode = orderCode, // You should use your own order reference here
                        BankCode = session?.PaymentMethodTypes?.FirstOrDefault(),
                        AppId = "Stripe",
                        Amount = (session?.AmountTotal ?? 0) / 100,
                        DiscountAmount = 0,
                        Checksum = session?.ClientReferenceId, // or metadata if you use it
                        Status = 1 // success
                    };
                    await _paymentService.HandleCallbackPaymentSuccessAsync(handleCallbackPaymentSuccessRequest);
                }
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
}