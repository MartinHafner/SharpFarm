using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SharpFarm;
using SharpFarm.Shared; // <-- hinzufügen für NavigationHelper

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var host = builder.Build();

// NavigationHelper initialisieren, damit BaseUri später im ScriptRunner funktioniert
NavigationHelper.Init(host.Services.GetRequiredService<NavigationManager>());

await host.RunAsync();
