namespace ReferenceGenerator;

internal sealed class ApplicationLifetime(IHostApplicationLifetime lifetime)
{
    private int _exitCode;
    public Int32 ExitCode => _exitCode;

    public void StopApplication(Int32 exitCode, out Boolean exitCodeWasSet)
    {
        exitCodeWasSet = Interlocked.CompareExchange(ref _exitCode, exitCode, 0) is 0;
        lifetime.StopApplication();
    }

    public void StopApplication(Int32 exitCode = 0) => StopApplication(exitCode, out _);
}
