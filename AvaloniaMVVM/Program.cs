using Avalonia;
using System;

namespace AvaloniaMVVM;

sealed class Program
{
    // Vor AppMain duerfen noch keine Avalonia- oder UI-abhaengigen APIs verwendet werden.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Diese Avalonia-Konfiguration wird auch vom visuellen Designer benoetigt.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}