namespace ScreenlyyIDSdk.Services.Instance
{
    public interface IDocumentService
    {
        Task<string> GetInstanceId(string correlationId);

        Task<bool> GetClassification(string instanceId, string correlationId);

		Task<string> PostDocumentImage(byte[] image, string instanceId, string correlationId, int side);
	}
}