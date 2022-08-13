using Microsoft.Maui.LifecycleEvents;
#if WINDOWS
using WinUIEx;
#endif


namespace CatFlasher;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FAS");
                fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "FAR");
                fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "FAB");
            });
#if WINDOWS
        builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(wndLifeCycleBuilder =>
                {
                    wndLifeCycleBuilder.OnWindowCreated(window =>
                    {
                        window.CenterOnScreen(830, 420);
                    });
                });
            });
#endif
        return builder.Build();
    }
}
