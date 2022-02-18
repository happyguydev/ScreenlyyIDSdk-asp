using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ScreenlyyIDSdk;
using ScreenlyyIDSdk.Services.Instance;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient<IDocumentService, DocumentService>();
builder.Services.AddHttpClient<ILivenessService, LivenessService>();
builder.Services.AddHttpClient<IFacematchService, FacematchService>();

await builder.Build().RunAsync();