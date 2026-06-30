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
using Microsoft.Extensions.DependencyInjection.Extensions;

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
      
        // Add services
        services.AddSingleton<ConfigService>();
        services.AddSingleton<MainNavigationService>();
        services.AddSingleton<SourceNavigationService>();
        services.AddSingleton<SourceManagementService>();

        // Add VMs
        services.AddSingleton<MainVm>();
        services.AddSingleton<StatusBarVm>();
        services.AddSingleton<AboutVm>();
        services.AddSingleton<AppSettingsVm>();
        services.AddSingleton<DashboardVm>();

        services.AddSingleton<SourceConfiguratorVm>();
        services.AddSingleton<RequestConfiguratorVm>();
        services.AddSingleton<DataDashboardVm>();
        services.AddSingleton<DashboardDesignerVm>();
        
        var provider = services.BuildServiceProvider();
        
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
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
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}