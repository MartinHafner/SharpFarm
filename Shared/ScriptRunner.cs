using System;
using System.Threading.Tasks;       // <-- for Task
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;


namespace SharpFarm.Shared;

public class ScriptRunner
{
    private GameWorld _world;

    public ScriptRunner(GameWorld world)
    {
        _world = world;
    }

    public async Task<string> RunAsync(string code)
    {
        var options = ScriptOptions.Default
            .AddReferences(typeof(GameWorld).Assembly)
            .AddImports("System", "System.Collections.Generic", "SharpFarm.Shared");

        try
        {
            string wrappedCode = $"var world = _world;\n{code}";
            await CSharpScript.EvaluateAsync(wrappedCode, options);
            return "OK";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
