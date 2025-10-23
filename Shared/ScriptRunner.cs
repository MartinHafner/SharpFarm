using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace SharpFarm.Shared;

public class ScriptRunner
{
    private readonly GameWorld _world;

    public ScriptRunner(GameWorld world)
    {
        _world = world;
    }

    public async Task<string> RunAsync(string code)
    {
        try
        {
            var globals = new Globals { world = _world };
            var options = ScriptOptions.Default
                .AddReferences(typeof(GameWorld).Assembly)
                .AddImports("System", "SharpFarm.Shared");

            await CSharpScript.EvaluateAsync(code, options, globals: globals);

            return "OK";
        }
        catch (Exception ex)
        {
            return $"Script error: {ex.Message}";
        }
    }

    public class Globals
    {
        public GameWorld world { get; set; } = null!;
    }
}
