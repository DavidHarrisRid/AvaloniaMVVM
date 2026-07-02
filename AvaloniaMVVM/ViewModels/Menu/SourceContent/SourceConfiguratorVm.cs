using AvaloniaMVVM.Models;
using AvaloniaMVVM.Services;
using AvaloniaMVVM.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaMVVM.ViewModels.Menu.SourceContent;

public partial class SourceConfiguratorVm : BaseVm
{
    private readonly SourceConfigurationService _sourceConfigurationService;

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

    public SourceConfiguratorVm(SourceConfigurationService sourceConfigurationService)
    {
        _sourceConfigurationService = sourceConfigurationService;
    }

    public void Load(ApiSourceModel? source)
    {
        _source = source;

        _sourceConfigurationService.Load(
            source,
            out var name,
            out var defaultBaseUrl,
            out var description,
            out var authenticationType,
            out var apiKey,
            out var defaultHeader);

        Name = name;
        DefaultBaseUrl = defaultBaseUrl;
        Description = description;
        AuthenticationType = authenticationType;
        ApiKey = apiKey;
        DefaultHeader = defaultHeader;
    }

    [RelayCommand]
    private void Save()
    {
        _sourceConfigurationService.Save(
            _source,
            Name,
            DefaultBaseUrl,
            Description,
            AuthenticationType,
            ApiKey,
            DefaultHeader);
    }
}