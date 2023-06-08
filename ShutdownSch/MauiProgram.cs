﻿using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Plugin.LocalNotification;

namespace ShutdownSch;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseLocalNotification()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

      

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
