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

        string[] dlls =
        {
            "Microsoft.CodeAnalysis.dll",
            "Microsoft.CodeAnalysis.CSharp.dll",
            "Microsoft.CodeAnalysis.Scripting.dll"
        };

        // GitHub Pages BaseAddress
        var http = new HttpClient { BaseAddress = new Uri("https://martinhafner.github.io/SharpFarm/") };

        foreach (var dll in dlls)
        {
            try
            {
                var url = $"roslyn/{dll}"; // relativ
                Console.WriteLine($"[Roslyn] Lade {http.BaseAddress}{url}...");
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

        if (_references == null || _references.Count == 0)
            return "Keine Roslyn-DLLs geladen!";

        var options = ScriptOptions.Default
            .AddReferences(_references)
            .AddImports("System", "System.Collections.Generic", "SharpFarm.Shared");

        try
        {
            var globals = new Globals { world = _world };
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
