using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvaloniaMVVM.Services;
using AvaloniaMVVM.ViewModels.Menu;

namespace AvaloniaMVVM.Behaviors;

public static class RequestTabDragDropBehavior
{
    // Das Attached Behavior uebersetzt Pointer-Ereignisse in ein MVVM-Kommando.
    private const string DragFormat = "application/x-avalonia-mvvm-request-tab";

    private static Point? _dragStartPoint;
    private static RequestWorkspaceTabVm? _draggedTab;
    private static bool _isDragging;

    public static readonly AttachedProperty<RequestWorkspaceTabVm?> TabProperty =
        AvaloniaProperty.RegisterAttached<Control, RequestWorkspaceTabVm?>(
            "Tab",
            typeof(RequestTabDragDropBehavior));

    public static readonly AttachedProperty<ICommand?> ReorderCommandProperty =
        AvaloniaProperty.RegisterAttached<Control, ICommand?>(
            "ReorderCommand",
            typeof(RequestTabDragDropBehavior));

    public static RequestWorkspaceTabVm? GetTab(Control control)
    {
        return control.GetValue(TabProperty);
    }

    public static void SetTab(Control control, RequestWorkspaceTabVm? value)
    {
        control.SetValue(TabProperty, value);
    }

    public static ICommand? GetReorderCommand(Control control)
    {
        return control.GetValue(ReorderCommandProperty);
    }

    public static void SetReorderCommand(Control control, ICommand? value)
    {
        control.SetValue(ReorderCommandProperty, value);
    }

    static RequestTabDragDropBehavior()
    {
        TabProperty.Changed.AddClassHandler<Control>(OnTabChanged);
    }

    private static void OnTabChanged(
        Control control,
        AvaloniaPropertyChangedEventArgs args)
    {
        control.RemoveHandler(
            InputElement.PointerPressedEvent,
            OnPointerPressed);

        control.RemoveHandler(
            InputElement.PointerMovedEvent,
            OnPointerMoved);

        control.RemoveHandler(
            InputElement.PointerReleasedEvent,
            OnPointerReleased);

        control.RemoveHandler(
            DragDrop.DragOverEvent,
            OnDragOver);

        control.RemoveHandler(
            DragDrop.DropEvent,
            OnDrop);

        if (args.NewValue is null)
        {
            return;
        }

        DragDrop.SetAllowDrop(control, true);

        // Auch von inneren Buttons behandelte Pointer-Ereignisse muessen den Drag starten koennen.
        control.AddHandler(
            InputElement.PointerPressedEvent,
            OnPointerPressed,
            RoutingStrategies.Tunnel | RoutingStrategies.Bubble,
            handledEventsToo: true);

        control.AddHandler(
            InputElement.PointerMovedEvent,
            OnPointerMoved,
            RoutingStrategies.Tunnel | RoutingStrategies.Bubble,
            handledEventsToo: true);

        control.AddHandler(
            InputElement.PointerReleasedEvent,
            OnPointerReleased,
            RoutingStrategies.Tunnel | RoutingStrategies.Bubble,
            handledEventsToo: true);

        control.AddHandler(
            DragDrop.DragOverEvent,
            OnDragOver,
            RoutingStrategies.Tunnel | RoutingStrategies.Bubble,
            handledEventsToo: true);

        control.AddHandler(
            DragDrop.DropEvent,
            OnDrop,
            RoutingStrategies.Tunnel | RoutingStrategies.Bubble,
            handledEventsToo: true);
    }

    private static void OnPointerPressed(
        object? sender,
        PointerPressedEventArgs args)
    {
        if (sender is not Control control)
        {
            return;
        }

        var pointer = args.GetCurrentPoint(control);

        if (!pointer.Properties.IsLeftButtonPressed)
        {
            return;
        }

        _dragStartPoint = args.GetPosition(control);
        _draggedTab = GetTab(control);
        _isDragging = false;
    }

    private static async void OnPointerMoved(
        object? sender,
        PointerEventArgs args)
    {
        if (sender is not Control control)
        {
            return;
        }

        if (_dragStartPoint is null || _draggedTab is null || _isDragging)
        {
            return;
        }

        var pointer = args.GetCurrentPoint(control);

        if (!pointer.Properties.IsLeftButtonPressed)
        {
            ResetDragState();
            return;
        }

        var currentPoint = args.GetPosition(control);
        var distance = currentPoint - _dragStartPoint.Value;

        // Der kleine Schwellwert verhindert, dass ein normaler Klick versehentlich einen Drag startet.
        if (Math.Abs(distance.X) < 6 && Math.Abs(distance.Y) < 6)
        {
            return;
        }

        _isDragging = true;

        var data = new DataObject();
        data.Set(DragFormat, _draggedTab);

        await DragDrop.DoDragDrop(
            args,
            data,
            DragDropEffects.Move);

        ResetDragState();
    }

    private static void OnPointerReleased(
        object? sender,
        PointerReleasedEventArgs args)
    {
        if (!_isDragging)
        {
            ResetDragState();
        }
    }

    private static void OnDragOver(
        object? sender,
        DragEventArgs args)
    {
        if (sender is not Control control)
        {
            return;
        }

        var targetTab = GetTab(control);

        if (_draggedTab is null ||
            targetTab is null ||
            _draggedTab == targetTab)
        {
            args.DragEffects = DragDropEffects.None;
            args.Handled = true;
            return;
        }

        args.DragEffects = DragDropEffects.Move;
        args.Handled = true;
    }

    private static void OnDrop(
        object? sender,
        DragEventArgs args)
    {
        if (sender is not Control control)
        {
            return;
        }

        var targetTab = GetTab(control);

        if (_draggedTab is null ||
            targetTab is null ||
            _draggedTab == targetTab)
        {
            ResetDragState();
            args.Handled = true;
            return;
        }

        var command = GetReorderCommand(control);
        var request = new TabReorderRequest(_draggedTab, targetTab);

        if (command?.CanExecute(request) == true)
        {
            command.Execute(request);
        }

        ResetDragState();
        args.Handled = true;
    }

    private static void ResetDragState()
    {
        _dragStartPoint = null;
        _draggedTab = null;
        _isDragging = false;
    }
}
