using System;
using System.Threading.Tasks;       // <-- for Task
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;


namespace SharpFarm.Shared;

public class ScriptRunner
{
    public async Task<string> RunAsync(string code, GameWorld world)
    {
        var options = ScriptOptions.Default
            .AddReferences(typeof(GameWorld).Assembly)
            .AddImports("System", "System.Collections.Generic", "SharpFarm.Shared");

        try
        {
            // globals-Anonyme Typen in Blazor WASM funktionieren nicht zuverlässig.
            // Wir übergeben world über eine bekannte Variable
            string wrappedCode = $"var world = (SharpFarm.Shared.GameWorld)DotNet.invokeMethod('SharpFarm', 'GetWorld');\n{code}";

            // In Blazor WebAssembly ggf. roslyn scripting DLLs müssen im wwwroot/roslyn liegen
            await CSharpScript.EvaluateAsync(wrappedCode, options);
            return "OK";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    // Für DotNet.invokeMethodAsync in JS
    [System.Runtime.InteropServices.ComVisible(true)]
    public static GameWorld GetWorld() => GameWorld.Instance;
}
