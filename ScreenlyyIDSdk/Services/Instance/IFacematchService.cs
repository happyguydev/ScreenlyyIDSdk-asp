namespace ScreenlyyIDSdk.Services.Instance;

public interface IFacematchService
{
    public Task<bool> ProcessFaceMatch(string image1, string correlationId);
}