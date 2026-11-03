using System.Globalization;

using Heiwase.App.Blazor;
using Heiwase.App.Blazor.Services;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
var apiBaseAddress = new Uri(string.IsNullOrWhiteSpace(apiBaseUrl) ? builder.HostEnvironment.BaseAddress : apiBaseUrl);
builder.Services.AddScoped<IHallOfFameService>(sp => new HallOfFameService(new HttpClient { BaseAddress = apiBaseAddress }));
builder.Services.AddRadzenComponents();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
var host = builder.Build();
var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
var result = await jsInterop.InvokeAsync<string>("localStorage.getItem", "blazorCulture");

CultureInfo culture;

if (result is not null)
{
    culture = new CultureInfo(result);
}
else
{
    culture = new CultureInfo("hu-HU");
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();
