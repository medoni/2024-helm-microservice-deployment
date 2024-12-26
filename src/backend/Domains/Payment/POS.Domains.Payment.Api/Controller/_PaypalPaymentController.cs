//using Asp.Versioning;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using PaypalServerSdk.Standard;
//using PaypalServerSdk.Standard.Models;
//using POS.Domains.Payment.Api.Dtos;

//namespace POS.Domains.Payment.Api.Controller;

///// <summary>
/////
///// </summary>
//[ApiVersion(1)]
//[ApiController]
//[Route("v1/Payment/Paypal")]
//public class PaypalPaymentController(
//    PaypalServerSdkClient PaypalClient
//) : ControllerBase
//{

//    /// <summary>
//    ///
//    /// </summary>
//    [HttpPost("BeginPayment")]
//    [ProducesResponseType<PaypalOrderDto>(StatusCodes.Status201Created)]
//    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
//    public async Task<IActionResult> BeginPayment(BeginPaymentDto dto)
//    {
//        var orderRequest = new OrdersCreateInput()
//        {
//            Body = new OrderRequest()
//            {
//                Intent = CheckoutPaymentIntent.Capture,
//                PurchaseUnits = new List<PurchaseUnitRequest>()
//                {
//                    new PurchaseUnitRequest()
//                    {
//                        Description = "Lorem Ipsum Description",
//                        Payee = new Payee() { EmailAddress = "sb-eg3vq35157431@business.example.com" },
//                        Amount = new AmountWithBreakdown("EUR", "42.42")
//                    }
//                },
//                ApplicationContext = new OrderApplicationContext()
//                {
//                    ReturnUrl = "https://ee58-193-176-121-136.ngrok-free.app/v1/Payment/Paypal/OnPaymentSuccess",
//                    CancelUrl = "https://ee58-193-176-121-136.ngrok-free.app/v1/Payment/Paypal/OnPaymentCancel"
//                }
//            }
//        };
//        var orderResponse = await PaypalClient.OrdersController.OrdersCreateAsync(orderRequest);

//        var orderDto = new PaypalOrderDto
//        {
//            PosOrderId = dto.OrderId,
//            PaypalOrderId = orderResponse.Data.Id,
//            PaypalLinks = orderResponse.Data.Links
//        };

//        await Task.Yield();
//        return Ok(orderDto);
//    }

//    /// <summary>
//    ///
//    /// </summary>
//    [HttpGet("OnPaymentSuccess")]
//    [ProducesResponseType(StatusCodes.Status200OK)]
//    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
//    public async Task<IActionResult> OnPaymentSuccess()
//    {
//        await Task.Yield();
//        return Ok();
//    }

//    /// <summary>
//    ///
//    /// </summary>
//    [HttpGet("OnPaymentCancel")]
//    [ProducesResponseType(StatusCodes.Status200OK)]
//    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
//    public async Task<IActionResult> OnPaymentCancel()
//    {
//        await Task.Yield();
//        return Ok();
//    }

//    /// <summary>
//    ///
//    /// </summary>
//    [HttpPost("CaptureOrder")]
//    [ProducesResponseType(StatusCodes.Status200OK)]
//    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
//    public async Task<IActionResult> CaptureOrder(CaptureOrderDto dto)
//    {
//        var captureInput = new OrdersCaptureInput()
//        {
//            Id = dto.PaypalOrderId
//        };

//        var captureResponse = await PaypalClient.OrdersController.OrdersCaptureAsync(captureInput);
//        if (captureResponse.StatusCode == (int)System.Net.HttpStatusCode.Created)
//        {

//        }

//        return Ok();
//    }
//}
