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

        // Core services
        services.AddSingleton<ConfigService>();
        services.AddSingleton<StatusBarService>();

        // Source/request services
        services.AddSingleton<SourceManagementService>();
        services.AddSingleton<SourceNavigationService>();
        services.AddSingleton<DashboardWorkspaceService>();
        services.AddSingleton<SourceConfigurationService>();
        services.AddSingleton<RequestConfigurationService>();

        // Main navigation
        services.AddSingleton<MainNavigationService>();

        // Root VMs
        services.AddSingleton<MainVm>();
        services.AddSingleton<StatusBarVm>();

        // Menu VMs
        services.AddSingleton<AboutVm>();
        services.AddSingleton<AppSettingsVm>();
        services.AddSingleton<DashboardVm>();

        // Source content VMs
        services.AddSingleton<SourceConfiguratorVm>();
        services.AddSingleton<RequestConfiguratorVm>();
        services.AddSingleton<DataDashboardVm>();
        services.AddSingleton<DashboardDesignerVm>();

        var provider = services.BuildServiceProvider();

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
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}