using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using AvaloniaMVVM.Services;
using AvaloniaMVVM.ViewModels;
using AvaloniaMVVM.ViewModels.Menu;
using AvaloniaMVVM.ViewModels.Menu.SourceContent;
using AvaloniaMVVM.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaMVVM;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();

        // Zentrale Anwendungsdienste werden als Singleton im DI-Container verwaltet.
        services.AddSingleton<ConfigService>();
        services.AddSingleton<StatusBarService>();

        // Diese Dienste bilden den Arbeitsbereich fuer Quellen und Anfragen ab.
        services.AddSingleton<SourceManagementService>();
        services.AddSingleton<SourceNavigationService>();
        services.AddSingleton<TabReorderService>();
        services.AddSingleton<DashboardWorkspaceService>();
        services.AddSingleton<DeleteConfirmationService>();
        services.AddSingleton<SourceConfigurationService>();
        services.AddSingleton<RequestConfigurationService>();

        // Die Hauptnavigation tauscht das aktuell angezeigte ViewModel aus.
        services.AddSingleton<MainNavigationService>();

        // Root-ViewModels leben waehrend der gesamten Anwendungslaufzeit.
        services.AddSingleton<MainVm>();
        services.AddSingleton<StatusBarVm>();

        // ViewModels der Hauptseiten werden ueber Dependency Injection aufgeloest.
        services.AddSingleton<AboutVm>();
        services.AddSingleton<AppSettingsVm>();
        services.AddSingleton<DashboardVm>();

        // Diese ViewModels stellen die wechselnden Inhalte des Dashboards bereit.
        services.AddSingleton<SourceConfiguratorVm>();
        services.AddSingleton<RequestConfiguratorVm>();
        services.AddSingleton<DataDashboardVm>();
        services.AddSingleton<DashboardDesignerVm>();

        var provider = services.BuildServiceProvider();

        // Erst beim Desktop-Lebenszyklus wird das Hauptfenster samt Root-DataContext erzeugt.
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            desktop.MainWindow = new MainView
            {
                DataContext = provider.GetRequiredService<MainVm>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Das Entfernen verhindert doppelte Validierung durch Avalonia und das MVVM-Toolkit.
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
