using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.Services;

public partial class ConfigService : ObservableObject
{
    // Zentrale Einstellungen bleiben dadurch unabhängig vom darstellenden ViewModel.
    [ObservableProperty]
    private string _apiUrl = string.Empty;

    public void UpdateSettings(string apiUrl)
    {
        ApiUrl = apiUrl;
    }
}
