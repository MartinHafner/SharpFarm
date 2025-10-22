using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace SharpFarm.Shared;

public class ScriptRunner
{
    private readonly GameWorld _world;
    private readonly IJSRuntime _js;

    public ScriptRunner(GameWorld world, IJSRuntime js)
    {
        _world = world;
        _js = js;
    }

    public async Task<string> RunAsync(string code)
    {
        try
        {
            // Lädt loader.js in /roslyn/ automatisch (GitHub Pages)
            await _js.InvokeVoidAsync("eval", @"
                if (!window.CSharpWasm) {
                    var s = document.createElement('script');
                    s.src = '/SharpFarm/roslyn/loader.js';
                    document.head.appendChild(s);
                }
            ");

            // Führe Code über CSharpWasm aus
            await _js.InvokeVoidAsync("CSharpWasm.runScript", code, _world);
            return "OK";
        }
        catch (JSException jsEx)
        {
            return $"JS error: {jsEx.Message}";
        }
        catch (Exception ex)
        {
            return $"Script error: {ex.Message}";
        }
    }
}
