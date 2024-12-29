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
    public async Task<IActionResult> GetPaymentDetails(Guid id)
    {
        var paymentDetails = await PaymentService.GetPaymentDetailsAsync(id);
        return Ok(paymentDetails);
    }

    /// <summary>
    /// Callback for the payment provider when the payment has been successfully processed.
    /// </summary>
    [HttpGet("{id}/OnSuccess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    public async Task<IActionResult> PaymentOnSuccessAsync(Guid id)
    {
        await PaymentService.OnSuccessfullyProcessedAsync(id);
        return Ok();
    }

    /// <summary>
    /// Callback for the payment provider when the payment was canceled.
    /// </summary>
    [HttpGet("{id}/OnCancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    public async Task<IActionResult> PaymentOnCancelAsync(Guid id)
    {
        await PaymentService.OnCanceledAsync(id);
        return Ok();
    }
}
