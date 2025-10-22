using System;
using System.Threading.Tasks;       // <-- for Task
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;


namespace SharpFarm.Shared;

public class ScriptRunner {
    public async Task<string> RunAsync(string code, GameWorld world) {
        var options = ScriptOptions.Default
            .AddReferences(typeof(GameWorld).Assembly)
            .AddImports("System", "System.Collections.Generic", "SharpFarm.Shared");

        try {
            await CSharpScript.EvaluateAsync(code, options, globals: new { world });
            return "OK";
        } catch (Exception ex) {
            return ex.Message;
        }
    }
}
