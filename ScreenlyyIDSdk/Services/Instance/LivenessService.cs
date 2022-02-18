using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ScreenlyyIDSdk.Services.Instance;

public class LivenessService : ILivenessService
{
    
    
    /// <summary>
    /// Local variables
    /// </summary>
    private readonly HttpClient _httpClient;
    private string baseUrl = "https://localhost:38418";  
    private string apiKey = "b43cd17a-cca2-45d2-858e-c0ee02f79907";
    
    
    /// <summary>
    /// class contructor
    /// </summary>
    /// <param name="httpClient">Http client DI</param>
    public LivenessService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ProcessLiveness(string image, string correlationId)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-api-key", this.apiKey);
            _httpClient.DefaultRequestHeaders.Add("x-correlation-id", correlationId);

            var body = new PassLiveRequest()
            {
                Image = image
            };
             
             var jsonRequest = JsonConvert.SerializeObject(body, Formatting.Indented, new JsonSerializerSettings
             {
                 NullValueHandling = NullValueHandling.Ignore
             });
             var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

             string url = $"{baseUrl}/api/v1/liveness";

          
            var response = await _httpClient.PostAsync(url, stringContent);
            var content = response.Content.ReadAsStringAsync();

            // should be saved to be used later
            var obj = content;
        }
        catch (Exception ex)
        {
        }

        // return
        return true;
    }
}

public class PassLiveRequest
{
    public Settings Settings { get; set; } = new Settings();
        
    public string Image { get; set; }
}

public class Settings
{
    public string SubscriptionId { get; set; }
        
    public Dictionary<string, string> AdditionalSettings { get; set; } = new Dictionary<string, string>();
}