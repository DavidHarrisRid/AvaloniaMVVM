using System.Collections.ObjectModel;
using AvaloniaMVVM.ViewModels.Menu;

namespace AvaloniaMVVM.Services;

public class TabReorderRequest
{
    // Das Kommando transportiert Ausgangs- und Ziel-Tab ohne UI-Abhaengigkeit zur Logik.
    public RequestWorkspaceTabVm DraggedTab { get; }

    public RequestWorkspaceTabVm TargetTab { get; }

    public TabReorderRequest(
        RequestWorkspaceTabVm draggedTab,
        RequestWorkspaceTabVm targetTab)
    {
        DraggedTab = draggedTab;
        TargetTab = targetTab;
    }
}

public class TabReorderService
{
    // Die ObservableCollection informiert die Tab-Leiste automatisch über die neue Reihenfolge.
    public void Reorder(
        ObservableCollection<RequestWorkspaceTabVm> tabs,
        TabReorderRequest request)
    {
        Reorder(tabs, request.DraggedTab, request.TargetTab);
    }

    public void Reorder(
        ObservableCollection<RequestWorkspaceTabVm> tabs,
        RequestWorkspaceTabVm draggedTab,
        RequestWorkspaceTabVm targetTab)
    {
        if (draggedTab == targetTab)
        {
            return;
        }

        var oldIndex = tabs.IndexOf(draggedTab);
        var newIndex = tabs.IndexOf(targetTab);

        if (oldIndex < 0 || newIndex < 0)
        {
            return;
        }

        tabs.Move(oldIndex, newIndex);
    }
}
