using AvaloniaMVVM.Services;

namespace AvaloniaMVVM.ViewModels.Menu;

public class AppSettingsVm(ConfigService configService) : BaseVm
{
    private readonly ConfigService _configService = configService;

    public string ApiUrl 
    { 
        get => _configService.ApiUrl;
        set
        {
            _configService.UpdateSettings(value);
            OnPropertyChanged();
        }
    }
}