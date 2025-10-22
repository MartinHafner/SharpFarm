using Microsoft.AspNetCore.Components;

namespace SharpFarm.Shared;

public static class NavigationHelper
{
    private static NavigationManager? _nav;
    public static string BaseUri => _nav?.BaseUri ?? "/";

    public static void Init(NavigationManager nav)
    {
        _nav = nav;
    }
}
