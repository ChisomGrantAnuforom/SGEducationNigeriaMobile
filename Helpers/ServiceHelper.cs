namespace SGEducationNigeriaMobile.Helpers;

using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceHelper
{
    public static T GetService<T>() => 
        Current.GetService<T>();

    public static IServiceProvider Current =>
        Application.Current.Handler.MauiContext.Services;
}
