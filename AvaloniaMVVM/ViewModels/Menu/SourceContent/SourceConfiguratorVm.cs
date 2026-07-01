using AvaloniaMVVM.Models;
using AvaloniaMVVM.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaMVVM.ViewModels.Menu.SourceContent;

public partial class SourceConfiguratorVm : BaseVm
{
    private StatusBarVm _statusBar;
    
    private ApiSourceModel? _source;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private string? _defaultBaseUrl;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private string? _authenticationType;

    [ObservableProperty]
    private string? _apiKey;

    [ObservableProperty]
    private string? _defaultHeader;

    [ObservableProperty]
    private string? _saveMessage;

    public SourceConfiguratorVm(StatusBarVm statusBar)
    {
        _statusBar = statusBar;
    }
    public void Load(ApiSourceModel? source)
    {
        _source = source;

        Name = source?.Name;
        DefaultBaseUrl = source?.DefaultBaseUrl;
        Description = source?.Description;
        AuthenticationType = source?.AuthenticationType;
        ApiKey = source?.ApiKey;
        DefaultHeader = source?.DefaultHeader;

        SaveMessage = null;
    }

    [RelayCommand]
    private void Save()
    {
        if (_source is null)
        {
            _statusBar.Message = "No source selected.";
            return;
        }

        _source.Name = Name;
        _source.DefaultBaseUrl = DefaultBaseUrl;
        _source.Description = Description;
        _source.AuthenticationType = AuthenticationType;
        _source.ApiKey = ApiKey;
        _source.DefaultHeader = DefaultHeader;

        _statusBar.Message = "Source saved.";
    }
}