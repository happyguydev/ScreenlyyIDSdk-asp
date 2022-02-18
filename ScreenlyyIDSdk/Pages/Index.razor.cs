using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ScreenlyyIDSdk.Services.Instance;

namespace ScreenlyyIDSdk.Pages;

public partial class Index
{
	/// <summary>
	/// Inject IJS runtime
	/// </summary>
	[Inject]
	protected IJSRuntime JS { get; set; }


	/// <summary>
	/// Inject document service
	/// </summary>
	[Inject]
	public IDocumentService DocumentService { get; set; }
	
	
	/// <summary>
	/// Inject document service
	/// </summary>
	[Inject]
	public ILivenessService LivenessService { get; set; }


	/// <summary>
	/// Local variables
	/// </summary>
	private string instanceId = "";
	private string correlationId="";
	private CaptureStep captureStep = CaptureStep.Front;

	private List<string> results = new();

	private string firstName;
	private string lastName;
	private string dob;


	/// <summary>
	/// Page life cycle inittialize eent
	/// </summary>
	/// <returns></returns>
	protected override async Task OnInitializedAsync()
	{
		await JS.InvokeVoidAsync("init", new { id_username = "user", id_password = "password" });
	}


	/// <summary>
	/// Starting camera
	/// </summary>
	/// <returns></returns>
	private async Task StartCamera() => await JS.InvokeVoidAsync("startCamera");


	/// <summary>
	/// Creating document instance
	/// </summary>
	/// <returns></returns>
	private async Task CreateDocumentInstance()
	{
		instanceId = await DocumentService.GetInstanceId(correlationId);
		instanceId = instanceId.Substring(1, instanceId.Length - 2);

		results.Add($"Instance Id: {instanceId}");
	}


	/// <summary>
	/// Getting document classification
	/// </summary>
	/// <returns></returns>
	private async Task GetClassification() => await DocumentService.GetClassification(instanceId, correlationId);


	/// <summary>
	/// Capture front-side id
	/// </summary>
	/// <returns></returns>
	private async Task CaptureFrontSideId()
	{
		correlationId = Guid.NewGuid().ToString();
		await this.CreateDocumentInstance();
		await this.StartCamera();
	}


	/// <summary>
	/// Capture back-side id
	/// </summary>
	/// <returns></returns>
	private async Task CaptureBackSideId() => await this.StartCamera();


	/// <summary>
	/// Using captured image
	/// </summary>
	/// <returns></returns>
	private async Task UseImage() => await JS.InvokeVoidAsync("getImage", DotNetObjectReference.Create(this));


	/// <summary>
	/// Posting image to server, called from js
	/// </summary>
	/// <param name="imageString">Image data</param>
	/// <returns></returns>
	[JSInvokable]
	public async Task PostDocumentImage(string imageString)
	{
		var sanitizedBase64 = imageString.Substring(imageString.LastIndexOf(',') + 1);
		byte[] imageData = Convert.FromBase64String(sanitizedBase64);

		int side = (captureStep ==  CaptureStep.Front) ? 0 : 1;

		var result = await DocumentService.PostDocumentImage(imageData, instanceId, correlationId, side);

		results.Add(result);

		if (captureStep == CaptureStep.Front) captureStep = CaptureStep.Back;
		else if (captureStep == CaptureStep.Back) captureStep = CaptureStep.Selfie;

		StateHasChanged();
	}

	[JSInvokable] 
	public async Task ProcessSelfie(string image)
	{
		var sanitizedBase64 = image.Substring(image.LastIndexOf(',') + 1);
		byte[] imageData = Convert.FromBase64String(sanitizedBase64);
		
		// TODO, call the new ProcessLiveness and ProcessFacematch services here
		await LivenessService.ProcessLiveness(image, correlationId);

	}
	
	
	
	
	

	private async Task TakeSelfie() => await JS.InvokeVoidAsync("openFrontCamera", DotNetObjectReference.Create(this));
	
}