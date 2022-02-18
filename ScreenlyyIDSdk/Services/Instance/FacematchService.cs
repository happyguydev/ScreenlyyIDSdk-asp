namespace ScreenlyyIDSdk.Services.Instance;

public class FacematchService : IFacematchService
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
    public FacematchService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<bool> ProcessFaceMatch(string image1, string correlationId)
    {
        // To process the facematch API you need 2 images:
        // 1. The image on the original ID that was scanned - this can be retrieved using the instance ID: 
        // 2. The selfie image just taken. So the selfie needs to be re-used here.



        throw new NotImplementedException();
    }
}

public class FaceMatchRequest
{
    public Settings Settings { get; set; } = new Settings();
        
    public ImageData Data { get; set; } = new ImageData();
}

public class ImageData
{
    public string ImageOne { get; set; }
        
    public string ImageTwo { get; set; }
}