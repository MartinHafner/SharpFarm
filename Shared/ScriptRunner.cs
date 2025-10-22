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

    /// <summary>
    /// Führt C# Code client-side via CSharpWasm aus.
    /// </summary>
    public async Task<string> RunAsync(string code)
    {
        try
        {
            // Übergabe von world via JS-Interop
            await _js.InvokeVoidAsync("CSharpWasm.runScript", code, DotNetObjectReference.Create(_world));
            return "OK";
        }
        catch (JSException jsEx)
        {
            return $"JS Error: {jsEx.Message}";
        }
        catch (System.Exception ex)
        {
            return $"Script Error: {ex.Message}";
        }
    }
}
