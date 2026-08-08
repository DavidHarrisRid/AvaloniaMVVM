using Avalonia;
using AvaloniaMVVM.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.Services;

public partial class DeleteConfirmationService : ObservableObject
{
    // Das vorgemerkte Modell wird erst nach der expliziten Bestaetigung geloescht.
    private readonly DashboardWorkspaceService _workspaceService;

    private ApiSourceModel? _pendingSource;
    private ApiRequestModel? _pendingRequest;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsBackgroundEnabled))]
    private bool _isOpen;
    
    public bool IsBackgroundEnabled => !IsOpen;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _message = string.Empty;

    public DeleteConfirmationService(DashboardWorkspaceService workspaceService)
    {
        _workspaceService = workspaceService;
    }

    public void OpenForSource(ApiSourceModel source)
    {
        _pendingSource = source;
        _pendingRequest = null;

        Title = GetString("DeleteSourceDialogTitle");
        Message = GetString("DeleteSourceDialogMessage");
        IsOpen = true;
    }

    public void OpenForRequest(ApiRequestModel request)
    {
        _pendingRequest = request;
        _pendingSource = null;

        Title = GetString("DeleteRequestDialogTitle");
        Message = GetString("DeleteRequestDialogMessage");
        IsOpen = true;
    }

    public void Cancel()
    {
        Clear();
    }

    public void Confirm()
    {
        if (_pendingSource is not null)
        {
            _workspaceService.DeleteSource(_pendingSource);
        }
        else if (_pendingRequest is not null)
        {
            _workspaceService.DeleteRequest(_pendingRequest);
        }

        Clear();
    }

    private void Clear()
    {
        // Nach Abbruch oder Bestaetigung wird der Dialogzustand vollstaendig zurueckgesetzt.
        _pendingSource = null;
        _pendingRequest = null;

        Title = string.Empty;
        Message = string.Empty;
        IsOpen = false;
    }

    private static string GetString(string key)
    {
        // Fehlt eine Ressource, bleibt ihr Schluessel als sichtbarer und diagnosefreundlicher Ersatz erhalten.
        if (Application.Current?.TryGetResource(key, null, out var value) == true &&
            value is string text)
        {
            return text.Trim();
        }

        return key;
    }
}
