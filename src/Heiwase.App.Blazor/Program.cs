using System.Globalization;

using Heiwase.App.Blazor;
using Heiwase.App.Blazor.Configuration;using Heiwase.App.Blazor.Services.Implementation;
using Heiwase.App.Blazor.Services.Interface;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var apiBaseUrl = builder.Configuration["ApiBaseUrl"]?.Trim();
if (string.IsNullOrEmpty(apiBaseUrl))
{
    apiBaseUrl = builder.HostEnvironment.BaseAddress;
}

if (!Uri.TryCreate(apiBaseUrl.EndsWith('/') ? apiBaseUrl : $"{apiBaseUrl}/", UriKind.Absolute, out var apiBaseAddress)
    || apiBaseAddress.Scheme is not ("http" or "https"))
{
    throw new InvalidOperationException($"ApiBaseUrl must be an absolute http(s) URL, for example 'https://func-name.azurewebsites.net/'. Current value: '{apiBaseUrl}'.");
}

var turnstileSiteKey = builder.Configuration["Turnstile:SiteKey"]?.Trim();
if (string.IsNullOrEmpty(turnstileSiteKey))
{
    throw new InvalidOperationException("Turnstile:SiteKey must be configured in appsettings.json.");
}

builder.Services.AddScoped<IHallOfFameService>(sp => new HallOfFameService(new HttpClient { BaseAddress = apiBaseAddress }));
builder.Services.AddScoped<IApplicationFormService>(sp => new ApplicationFormService(new HttpClient { BaseAddress = apiBaseAddress }));
builder.Services.AddSingleton(new TurnstileSettings(turnstileSiteKey));
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
