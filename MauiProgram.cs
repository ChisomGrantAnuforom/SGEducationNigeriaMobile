using Microsoft.Extensions.Logging;
using SGEducationNigeriaMobile.Pages;
using SGEducationNigeriaMobile.Pages.User;
using SGEducationNigeriaMobile.Services;
using CommunityToolkit.Maui;

namespace SGEducationNigeriaMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        


        // ✅ Register HttpClient + ApiService
        builder.Services.AddHttpClient<ApiService>(); 

        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegistrationWizardPage>();
        builder.Services.AddTransient<UserHomePage>();
        builder.Services.AddTransient<DocumentUploadPage>();
        
        // builder.Services.AddSingleton<HttpClient>();
        // builder.Services.AddSingleton<ApiService>();


        builder.Services.AddTransient<AdmissionApplicationWizardPage>();
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}