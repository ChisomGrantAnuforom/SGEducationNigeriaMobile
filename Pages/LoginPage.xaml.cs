using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SGEducationNigeriaMobile.Pages.User;
using SGEducationNigeriaMobile.Services;

namespace SGEducationNigeriaMobile.Pages;

public partial class LoginPage : ContentPage
{
    
    private readonly ApiService _apiService;
    
    public LoginPage( ApiService apiService)
    {
        
        InitializeComponent();
        
        _apiService = apiService;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {


        var email = EmailEntry.Text;
        var password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
        {
            await DisplayAlert("Error", "Please enter a valid email.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Password is required.", "OK");
            return;
        }


        try
        {

            LoginButton.IsEnabled = false;
            LoginLoader.IsVisible = true;
            LoginLoader.IsRunning = true;
            

            var response = await _apiService.Login(EmailEntry.Text, PasswordEntry.Text);

            if (response != null)
            {
                SessionManager.StudentId = response.Id;
                SessionManager.StudentName = response.FirstName + " " + response.Surname;

                Console.WriteLine("Student ID::: " + SessionManager.StudentId);

                // await DisplayAlert("Success", "Logged in!", "OK");

                // var dashboard = App.Current.Handler.MauiContext.Services
                //     .GetService<DashboardPage>(); 
                //
                // await Navigation.PushAsync(dashboard);

                // Application.Current.MainPage = new AppShell();

                // To switch to a different Shell file named "UserShell"
                Application.Current.MainPage = new UserShell();

                // // Navigate to Dashboard
                // await Shell.Current.GoToAsync("//DashboardPage");
            }
            else
            {
                await DisplayAlert("Error", "Invalid login", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            LoginLoader.IsRunning = false;
            LoginLoader.IsVisible = false;
            LoginButton.IsEnabled = true;
        }

    }

    private async void OnForgotPasswordTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Forgot Password", "Reset link coming soon.", "OK");
    }

    private async void OnSignUpTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Sign Up", "Navigate to registration page.", "OK");
    }

    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase);
    }
} 