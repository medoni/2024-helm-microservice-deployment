using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Domains.Payment.Service;
using POS.Domains.Payment.Service.Dtos;

namespace POS.Domains.Payment.Api.Controller;
/// <summary>
/// Responsible for handling payment requests
/// </summary>
[ApiVersion(1)]
[ApiController]
[Route("v1/Payment")]
public class PaymentController(
    IPaymentService PaymentService
) : ControllerBase
{
    /// <summary>
    /// Requests payment for a given entity.
    /// </summary>
    [HttpPost("Request")]
    [ProducesResponseType<PaymentDetailsDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> RequestPaymentAsync(RequestPaymentDto dto)
    {
        var paymentDetails = await PaymentService.RequestPaymentAsync(dto);
        return Ok(paymentDetails);
    }

    /// <summary>
    /// Trying to capture a payment that was already paid.
    /// </summary>
    [HttpPost("Capture")]
    [ProducesResponseType<PaymentDetailsDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    public async Task<IActionResult> CapturePaymentAsync(CapturePaymentDto dto)
    {
        var paymentDetails = await PaymentService.CapturePaymentAsync(dto);
        return Ok(paymentDetails);
    }

    /// <summary>
    /// Returns details of the payment.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType<PaymentDetailsDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    public async Task<IActionResult> GetPaymentDetails(Guid paymentId)
    {
        var paymentDetails = await PaymentService.GetPaymentDetailsAsync(paymentId);
        return Ok(paymentDetails);
    }
}
