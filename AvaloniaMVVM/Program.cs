using Avalonia;
using System;

namespace AvaloniaMVVM;

sealed class Program
{
    // Vor AppMain duerfen noch keine Avalonia- oder UI-abhängigen APIs verwendet werden.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Diese Avalonia-Konfiguration wird auch vom visuellen Designer benötigt.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}