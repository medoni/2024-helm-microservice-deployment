using POS.Domains.Payment.Service.Domain;

namespace POS.Domains.Payment.Service.Processors;
internal class PaymentProviderCaptureResultDto
{
    public required PaymentProviderState ProviderState { get; init; }
    public required DateTimeOffset PayedAt { get; init; }
    public required DateTimeOffset CapturedAt { get; init; }
}
