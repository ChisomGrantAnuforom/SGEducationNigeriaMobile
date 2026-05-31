using Microsoft.Maui.Controls;
using SGEducationNigeriaMobile.Pages;

namespace SGEducationNigeriaMobile;


public partial class MainPage : ContentPage
{
    int count = 0;
    
    

    public MainPage()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
    }

    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
        
        
       // await Shell.Current.GoToAsync(nameof(RegisterPage)); 
       //  Console.WriteLine("RegisterPage Clicked");
       
       
       var registerPage = App.Current.Handler.MauiContext.Services
           .GetService<RegisterPage>(); 

       await Navigation.PushAsync(registerPage);
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        // Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
       await  Shell.Current.GoToAsync(nameof(LoginPage)); 
        Console.WriteLine("LoginPage Clicked");
    }
}