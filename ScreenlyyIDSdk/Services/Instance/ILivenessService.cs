namespace ScreenlyyIDSdk.Services.Instance;

public interface ILivenessService
{
    Task<bool> ProcessLiveness(string image, string correlationId);
}