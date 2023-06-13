using Microsoft.Win32;

namespace EasyWindowsAutoScheduler;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

        var appName = "Easy Windows Auto Scheduler";
        string appPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

        using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
        {
            key.SetValue(appName, appPath);
        }
    }


    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        const int newWidth = 700;
        const int newHeight = 400;

        window.Width = newWidth;
        window.Height = newHeight;

        return window;
    }
  
}
