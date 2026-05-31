using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroidX.AppCompat.App;
using SGEducationNigeriaMobile.Helpers;
using SGEducationNigeriaMobile.Pages;
using SGEducationNigeriaMobile.Pages.User;
using SGEducationNigeriaMobile.Services;

namespace SGEducationNigeriaMobile;

public partial class UserShell : Shell
{
    public UserShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        
        
    }


    private readonly ApiService _apiService;

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        try
        {

            Preferences.Remove("StudentId");
            // await Shell.Current.GoToAsync(nameof(LoginPage));

            // Application.Current.MainPage = new LoginPage(_apiService);
            
            
            Application.Current.MainPage = ServiceHelper.GetService<LoginPage>(); 

            // await Shell.Current.GoToAsync("//LoginPage");  
        }
        catch (Exception ex)
        {
            Console.WriteLine("EEEERRRRRRRROOOOORRRRR:::::   "+ex.Message);
            DisplayAlert("Exception", "EEEERRRRRRRROOOOORRRRR:::::   " + ex.Message, "OK");
        }
    }


}