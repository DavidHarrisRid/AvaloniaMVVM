using System.ComponentModel;
using AvaloniaMVVM.Services;

namespace AvaloniaMVVM.ViewModels.Menu;

public class AppSettingsVm : BaseVm
{
    private readonly ConfigService _configService;

    public string ApiUrl
    {
        get => _configService.ApiUrl;
        set => _configService.UpdateSettings(value);
    }

    public AppSettingsVm(ConfigService configService)
    {
        _configService = configService;
        _configService.PropertyChanged += OnConfigServicePropertyChanged;
    }

    private void OnConfigServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ConfigService.ApiUrl))
        {
            OnPropertyChanged(nameof(ApiUrl));
        }
    }
}