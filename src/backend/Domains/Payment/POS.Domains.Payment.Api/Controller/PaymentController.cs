using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Domains.Payment.Api.Dtos;
using POS.Domains.Payment.Service.Services.PaymentProcessor;

namespace POS.Domains.Payment.Api.Controller;
/// <summary>
/// Responsible for handling payment requests
/// </summary>
[ApiVersion(1)]
[ApiController]
[Route("v1/Payment")]
public class PaymentController(
    IPaymentProcessor paymentProcessor
) : ControllerBase
{
    /// <summary>
    /// Requests payment for a given entity.
    /// </summary>
    [HttpPost("Request")]
    [ProducesResponseType<PaymentDetailsDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> RequestPaymentAsync(RequestPaymentDto dto)
    {
        var payment = await paymentProcessor.RequestPaymentAsync(
            dto.Provider,
            dto.EntityType,
            dto.EntityId,
            dto.RequestedAt
        );
        var paymentDetails = payment.ToPaymentDetailsDto();

        return Ok(paymentDetails);
    }

    /// <summary>
    /// Trying to capture a payment that was already paid.
    /// </summary>
    [HttpPost("Capture")]
    [ProducesResponseType<PaymentDetailsDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    public Task<IActionResult> CapturePaymentAsync(CapturePaymentDto dto)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns details of the payment.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType<PaymentDetailsDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    public Task<IActionResult> GetPaymentDetails(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Callback for the payment provider when the payment has been successfully processed.
    /// </summary>
    [HttpGet("{id}/OnSuccess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    public Task<IActionResult> PaymentOnSuccessAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Callback for the payment provider when the payment was canceled.
    /// </summary>
    [HttpGet("{id}/OnCancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    public Task<IActionResult> PaymentOnCancelAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
