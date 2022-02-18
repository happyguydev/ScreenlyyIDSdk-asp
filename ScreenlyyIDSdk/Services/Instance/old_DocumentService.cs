using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScreenlyyIDSdk.Services.Instance;

public class old_DocumentService : old_IDocumentService
{
    private readonly HttpClient _httpClient;

    public old_DocumentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<string> GetInstanceId()
    {
        var request = new
        {
            Authenticationsensitivity = 0,
            ClassificationMode = 0,
            Device = new
            {
                HasContactlessChipReader = false,
                HasMagneticStripeReader = false,
                SerialNumber = "JavaScriptWebSDK ",
                Type = new
                {
                    Manufacturer = "xxx",
                    Model = "xxx",
                    SensorType = 3,
                }
            },
            ImageCroppingExpectedSize = 0,
            ImageCroppingMode = 0,
            ManualDocumentType = (object) null,
            ProcessMode = 0,
            SubscriptionId = ""
        };

        string json = JsonSerializer.Serialize(request);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        
        _httpClient.DefaultRequestHeaders.Add("x-api-key", "b43cd17a-cca2-45d2-858e-c0ee02f79907");
        var response = await _httpClient.PostAsync("https://test-api.screenlyyid.com/document/instance", stringContent);
        var content = response.Content.ReadAsStringAsync();

        // the instanceID here should be saved to be used later
        return "";
        //throw new NotImplementedException();
    }
        
    public async Task PostDocumentImage(byte[] image, string instanceId)
    {
        
        
        //TODO should really default this to be global
        _httpClient.DefaultRequestHeaders.Add("x-api-key", "b43cd17a-cca2-45d2-858e-c0ee02f79907");

        var formContent = new MultipartFormDataContent();
        var imageContent = new ByteArrayContent(image);
        formContent.Add(imageContent);
        
        var response = await _httpClient.PostAsync($"https://test-api.screenlyyid.com/document/{instanceId}/image", formContent);
    }
}