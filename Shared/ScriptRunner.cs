using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace SharpFarm.Shared;

public class ScriptRunner
{
    private readonly GameWorld _world;
    private static List<MetadataReference>? _references;
    private static bool _initialized = false;

    public ScriptRunner(GameWorld world)
    {
        _world = world;
    }

    private static async Task EnsureReferencesLoadedAsync()
    {
        if (_initialized) return;
        _initialized = true;
        _references = new List<MetadataReference>();

        // Diese DLLs kopiert deine GitHub Action in wwwroot/roslyn/
        string[] roslynDlls = new[]
        {
            "Microsoft.CodeAnalysis.dll",
            "Microsoft.CodeAnalysis.CSharp.dll",
            "Microsoft.CodeAnalysis.Scripting.dll"
        };

        var http = new HttpClient();

        foreach (var dll in roslynDlls)
        {
            try
            {
                var url = $"/roslyn/{dll}";
                var bytes = await http.GetByteArrayAsync(url);
                _references.Add(MetadataReference.CreateFromStream(new MemoryStream(bytes)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden von {dll}: {ex.Message}");
            }
        }
    }

    public async Task<string> RunAsync(string code)
    {
        await EnsureReferencesLoadedAsync();

        var options = ScriptOptions.Default
            .AddReferences(_references!)
            .AddImports("System", "System.Collections.Generic", "SharpFarm.Shared");

        try
        {
            var globals = new Globals { world = _world };
            await CSharpScript.EvaluateAsync(code, options, globals: globals);
            return "OK";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }

    public class Globals
    {
        public GameWorld world { get; set; } = null!;
    }
}
