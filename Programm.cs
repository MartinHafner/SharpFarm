using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SharpFarm;
using SharpFarm.Shared;
using Microsoft.AspNetCore.Components;         // NavigationManager
using Microsoft.Extensions.DependencyInjection; // GetRequiredService<T>()

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var host = builder.Build();

// NavigationHelper initialisieren
var nav = host.Services.GetRequiredService<NavigationManager>();
NavigationHelper.Init(nav);

await host.RunAsync();
