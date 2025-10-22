using System;
using System.Threading.Tasks;       // <-- for Task
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;


namespace SharpFarm.Shared;

public class ScriptGlobals
{
    public GameWorld world { get; set; }
}

public class ScriptRunner {
    public async Task<string> RunAsync(string code, GameWorld world) {
        var codeWithLog = "using System;\n" + code + "\nConsole.WriteLine(\"Script ran\");";
        var options = ScriptOptions.Default
            .AddReferences(typeof(GameWorld).Assembly)
            .AddImports("System", "System.Collections.Generic", "SharpFarm.Shared");
        var globals = new ScriptGlobals { world = world };
        try {
            await CSharpScript.EvaluateAsync(code, options, globals: globals);
            return "OK";
        } catch (Exception ex) {
            return ex.Message;
        }
    }
}
