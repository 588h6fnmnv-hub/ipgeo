namespace ipgeo.UI.Components;

using ipgeo.UI;
using System.Threading;

public class Spinner : IDisposable
{
    private readonly string _message;
    private readonly string[] _frames;
    private readonly int _delayMs;
    private readonly CancellationTokenSource _cts;
    private readonly Task _animationTask;
    private int _frameIndex = 0;
    private bool _disposed = false;

    public Spinner(string message, string[]? frames = null, int delayMs = 80)
    {
        _message = message;
        _frames = frames ?? Theme.SpinnerHacker;
        _delayMs = delayMs;
        _cts = new CancellationTokenSource();
        
        Console.Write($"{Theme.Colors.Cyan}  {Theme.Icons.Radar}  {_message} {Theme.Colors.Reset} ");
        _animationTask = Task.Run(AnimateAsync, _cts.Token);
    }

    private async Task AnimateAsync()
    {
        try
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                var frame = _frames[_frameIndex % _frames.Length];
                Console.Write($"\r{Theme.Colors.Cyan}{frame}{Theme.Colors.Reset} ");
                _frameIndex++;
                await Task.Delay(_delayMs, _cts.Token);
            }
        }
        catch (OperationCanceledException) { }
        catch { }
    }

    public void Stop(string? finalMessage = null)
    {
        if (_disposed) return;
        
        _cts.Cancel();
        try { _animationTask.Wait(100); } catch { }
        
        Console.Write("\r" + new string(' ', Console.WindowWidth > 0 ? Console.WindowWidth - 1 : 80) + "\r");
        
        if (!string.IsNullOrEmpty(finalMessage))
        {
            Console.WriteLine($"{Theme.Prefix.Success}{finalMessage}{Theme.Colors.Reset}");
        }
        
        _disposed = true;
    }

    public void Dispose()
    {
        Stop();
        _cts.Dispose();
    }
}

public class ProgressBar : IDisposable
{
    private readonly int _total;
    private int _current;
    private readonly string _message;
    private readonly int _width;
    private bool _disposed = false;

    public int Current => _current;

    public ProgressBar(int total, string message, int width = 40)
    {
        _total = total;
        _message = message;
        _width = width;
        _current = 0;
        Draw(0);
    }

    public void Increment(int amount = 1)
    {
        if (_disposed) return;
        _current = Math.Min(_current + amount, _total);
        Draw(_current);
    }

    public void SetProgress(int current)
    {
        if (_disposed) return;
        _current = Math.Min(Math.Max(current, 0), _total);
        Draw(_current);
    }

    private void Draw(int current)
    {
        var percent = _total > 0 ? (double)current / _total : 0;
        var filled = (int)(_width * percent);
        var empty = _width - filled;
        
        var bar = new string(Theme.ProgressBar.Fill, filled) + new string(Theme.ProgressBar.FillDim, empty);
        var percentStr = $"{(percent * 100):F0}%".PadLeft(4);
        
        Console.Write($"\r{Theme.Colors.Cyan}{Theme.Icons.Radar}{Theme.Colors.Reset} {_message} ");
        Console.Write($"{Theme.Colors.PrimaryDim}{Theme.ProgressBar.Left}{Theme.Colors.Reset}{Theme.Colors.Primary}{new string(Theme.ProgressBar.Fill, filled)}{Theme.Colors.PrimaryDim}{new string(Theme.ProgressBar.FillDim, empty)}{Theme.Colors.Reset}{Theme.Colors.PrimaryDim}{Theme.Colors.Reset} {Theme.Colors.Cyan}{percentStr,4}{Theme.Colors.Reset} ");
    }

    public void Complete(string? finalMessage = null)
    {
        if (_disposed) return;
        
        Draw(_total);
        Console.WriteLine();
        
        if (!string.IsNullOrEmpty(finalMessage))
        {
            Console.WriteLine($"{Theme.Prefix.Success}{finalMessage}{Theme.Colors.Reset}");
        }
        
        _disposed = true;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Complete();
        }
    }
}

public static class Loading
{
    public static async Task<T> WithSpinner<T>(string message, Func<Task<T>> action, string[]? frames = null, int delayMs = 80)
    {
        using var spinner = new Spinner(message, frames);
        try
        {
            var result = await action();
            spinner.Stop($"Done!");
            return result;
        }
        catch (Exception ex)
        {
            spinner.Stop($"Failed: {ex.Message}");
            throw;
        }
    }

    public static async Task WithSpinner(string message, Func<Task> action, string[]? frames = null, int delayMs = 80)
    {
        using var spinner = new Spinner(message, frames);
        try
        {
            await action();
            spinner.Stop("Done!");
        }
        catch (Exception ex)
        {
            spinner.Stop($"Failed: {ex.Message}");
            throw;
        }
    }

    public static async Task<T> WithProgress<T>(int total, string message, Func<IProgress<int>, Task<T>> action)
    {
        using var progress = new ProgressBar(total, message);
        var progressReporter = new Progress<int>(v => progress.Increment(v - progress.Current));
        
        try
        {
            var result = await action(progressReporter);
            progress.Complete("Done!");
            return result;
        }
        catch
        {
            progress.Dispose();
            throw;
        }
    }
}