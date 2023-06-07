using Microsoft.Win32;

namespace ShutdownSch;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override void OnStart()
    {
        var appName = "Easy Windows Auto Scheduler";
        string appPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

        using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
        {
            key.SetValue(appName, appPath);
        }

        // The rest of your OnStart code...
    }
}
