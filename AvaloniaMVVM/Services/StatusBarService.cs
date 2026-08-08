using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.Services;

public partial class StatusBarService : ObservableObject
{
    // Jede neue Meldung startet den zehnsekundigen Ruecksetz-Timer erneut.
    private const string IdleMessage = "...";
    private static readonly TimeSpan ResetDelay = TimeSpan.FromSeconds(10);

    private CancellationTokenSource? _resetCancellationTokenSource;

    [ObservableProperty]
    private string _message = IdleMessage;

    public void SetMessage(string message)
    {
        Message = message;

        if (message == IdleMessage)
        {
            CancelResetTimer();
            return;
        }

        RestartResetTimer();
    }

    private void RestartResetTimer()
    {
        // Nur der Timer der neuesten Meldung darf die Statusleiste spaeter zuruecksetzen.
        CancelResetTimer();

        _resetCancellationTokenSource = new CancellationTokenSource();

        _ = ResetMessageAfterDelayAsync(_resetCancellationTokenSource.Token);
    }

    private void CancelResetTimer()
    {
        _resetCancellationTokenSource?.Cancel();
        _resetCancellationTokenSource?.Dispose();
        _resetCancellationTokenSource = null;
    }

    private async Task ResetMessageAfterDelayAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(ResetDelay, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            // Bindbare UI-Daten werden nach dem Hintergrund-Timer auf dem UI-Thread aktualisiert.
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    Message = IdleMessage;
                }
            });
        }
        catch (TaskCanceledException)
        {
            // Eine neuere Statusmeldung hat den vorherigen Timer ersetzt.
        }
        catch (OperationCanceledException)
        {
            // Eine neuere Statusmeldung hat den vorherigen Timer ersetzt.
        }
    }
}
