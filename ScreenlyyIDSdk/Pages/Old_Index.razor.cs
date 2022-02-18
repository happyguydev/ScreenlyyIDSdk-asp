using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ScreenlyyIDSdk.Services.Instance;

namespace ScreenlyyIDSdk.Pages;

public partial class old_Index
{
    
    [Inject]
    protected  IJSRuntime JS { get; set; }
    
    [Inject]
    public IDocumentService DocumentService { get; set; }
    
    
       // **** APP FLOW *****************
    // This app captures the users ID, front, then back, sends its to a set of APIs to validate its a valid ID. Then asks for a selfie, where it calls some more APIs to compare
    // the selfie image to the image on the ID.
    //
    // it usues a JS API contained in the /wwwroot/js folder. This is an Acuant API. You can see it started already below using the Initialize() method.
    // there are wrapper methods contained in the base.js file.
    //
    // Below is the basic flow this app needs to perform:
    //
    // 1. Button on UI to start camera - this will interact with the Acuant JS API and open device camera. This is already started in StartCamera() method below.
    //          - user should capture front of their ID at this point (for example the front of your drivers licence)
    // 2. Capture the photo, then display the image in the UI - see base.js. The startCamera js method eventually puts the captured image on a canvas HTML element (this needs to fit the screen and be responsive).
    // 3. While this is happening, there is a call to CreateDocumentInstance() - this calls the API and returns an ID/GUID. This GUID needs to be saved in session storage and will be used several times later in the flow.
    // 4. Once the image of the front of the ID is rendered on screen, have a button that says "Use this image". When the user clicks this button POST the image to the following API: https://test-api.screenlyyid.com/Document/{instanceID}/Classification
    //      - you will get back a json response of this image. For the puropse of this POC dont worry about saving this information. Ill add that in later.
    // 5. Once you get the results back in step 4, hide the image on the UI, and show a button: "Capture Back of ID"
    // 6. When the user clicks the button, you will need to start the camera again, the same as step 2. At this point the user should capture the back of their ID. It needs to be displayed on the UI, the same as step 2
    // 7. Repeat step 4 (step 3 should be ignored and not repeated here).
    // 8. Once the result is back from step 7, clear the UI and show a button "Take Selfie Image". The javascrip API to start the selfie can be found in the react app in the CaptureSelfie.js file
    //                                openFrontCamera() {
    //                                      window.AcuantPassiveLiveness.startSelfieCapture(this.onCaptured.bind(this));
    //                                      }
    // 9. While this is happening, call this API: https://test-api.screenlyyid.com/Document/{InstanceID}0ed8f4c1-3439-42e8-8e4b-7b98ab8195e2}/Field/Image?key=Photo - this will return an image that you will need to store and use later.
    // 10. The javascript API will return the selfie image to you. The base64 encided image then needs to be sent to this API: https://test-api.screenlyyid.com/api/v1/liveness - this API will give you a json response back. Just save this blob locally for now, ill refine it later and map it to a class later for other uses.
    // 11. Once you get a response back from step 10, you can make another call to this API: https://test-api.screenlyyid.com/api/v1/facematch in the paylod of this image you will need to send 2 images. 1. the base64encoded image returned from step 8 (the selfie image), and the base64encoded image from step 9 (the photo in the ID). This will return a json result.
    // 12. once you get this response. Display a form that the user can input firstName, lastName, DOB, and a submit button
    
    // thats all thats needed for this POC.
    // one thing to ensure works is the images when displayed in the UI are responsive.
    
    
    
    
    
    
    private async Task Initialize()
    {
        await JS.InvokeVoidAsync("init", new { id_username = "user", id_password="password" });
    }


    private async Task UseImage()
    {
        // call JS to get the image from the canvas
        // from JS pass back to C#
        
        // send the image to the API 
        // determine if its back or front of image
        // get the results from API and store in local storage
        
        await JS.InvokeVoidAsync("getImage", DotNetObjectReference.Create(this));
    }

    [JSInvokable]
    public async Task ProcessImage(string imageString)
    {
        var instanceId = "";
        var sanitizedBase64 = imageString.Substring(imageString.LastIndexOf(',') + 1);
        byte[] imageData = Convert.FromBase64String(sanitizedBase64);
        //await DocumentService.PostDocumentImage(imageData, instanceId);

    }
    
    
 
    
    
    
    
    public async Task StartCamera()
    {
        await StartCameraJs();
        await CreateDocumentInstance();
    }
    
    private async Task CreateDocumentInstance()
    {
        //await DocumentService.GetInstanceId();
    }
    
    private async Task StartCameraJs()
    {
        await JS.InvokeVoidAsync("startCamera");
    }

  

    //protected override async Task OnInitializedAsync()
    //{
    //    await Initialize();
    //}

  
}