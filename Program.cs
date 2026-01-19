using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Heiwase.Components.Pages; // FONTOS: Ellenõrizd, hogy a Home.razor tényleg ebben a névtérben van-e!

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// A komponensek becsatolása a HTML-be (index.html id="app")
builder.RootComponents.Add<Home>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient beállítása (ez kell a JSON betöltéshez)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Indítás
await builder.Build().RunAsync();