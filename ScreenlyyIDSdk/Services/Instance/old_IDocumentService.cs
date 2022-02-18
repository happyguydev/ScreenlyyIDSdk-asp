namespace ScreenlyyIDSdk.Services.Instance;

public interface old_IDocumentService
{
    Task<string> GetInstanceId();
    Task PostDocumentImage(byte[] image, string instanceId);
}