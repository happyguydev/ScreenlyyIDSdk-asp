using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ScreenlyyIDSdk.Services.Instance
{
	public class DocumentService : IDocumentService
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
		public DocumentService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}


		/// <summary>
		/// Getting document instance id
		/// </summary>
		/// <returns></returns>
		public async Task<string> GetInstanceId(string correlationId)
		{
			string instanceId = "";

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
				ManualDocumentType = (object)null,
				ProcessMode = 0,
				SubscriptionId = ""
			};

			try
			{
				string json = System.Text.Json.JsonSerializer.Serialize(request);
				var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

				_httpClient.DefaultRequestHeaders.Clear();
				_httpClient.DefaultRequestHeaders.Add("x-api-key", this.apiKey);
				_httpClient.DefaultRequestHeaders.Add("x-correlation-id", correlationId);

				var response = await _httpClient.PostAsync($"{this.baseUrl}/document/instance", stringContent);
				var content = response.Content.ReadAsStringAsync();

				// the instanceID here should be saved to be used later
				instanceId = content.Result;
			}
			catch (Exception ex)
			{
			}

			// return
			return instanceId;
		}


		/// <summary>
		/// Getting document classification
		/// </summary>
		/// <returns></returns>
		public async Task<bool> GetClassification(string instanceId, string correlationId)
		{
			try
			{
				_httpClient.DefaultRequestHeaders.Clear();
				_httpClient.DefaultRequestHeaders.Add("x-api-key", this.apiKey);
				_httpClient.DefaultRequestHeaders.Add("x-correlation-id", correlationId);

				var response = await _httpClient.PostAsync($"{baseUrl}/document/{instanceId}/classification", null);
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


		/// <summary>
		/// Posting image tover
		/// </summary>
		/// <param name="image">Image data</param>
		/// <param name="instance Id">Instance id</param>
		/// <param name="correlation Id">correlationid</param>
		/// <returns></returns>
		public async Task<string> PostDocumentImage(byte[] image, string instanceId, string correlationId, int side)
		{
			var result="";

			try
			{
				_httpClient.DefaultRequestHeaders.Clear();
				_httpClient.DefaultRequestHeaders.Add("x-api-key", this.apiKey);
				_httpClient.DefaultRequestHeaders.Add("x-correlation-id", correlationId);


				var formContent = new MultipartFormDataContent();
				var imageContent = new ByteArrayContent(image);
				formContent.Add(imageContent, "file1", "file1");

				string url = $"{this.baseUrl}/document/{instanceId}/image?side={side}&light=0&metrics=true";
				var response = await _httpClient.PostAsync(url, formContent);
				var content = response.Content.ReadAsStringAsync();

				result= JsonConvert.SerializeObject(content);
			}
			catch (Exception ex)
			{
			}

			return result;
		}
	}
}

