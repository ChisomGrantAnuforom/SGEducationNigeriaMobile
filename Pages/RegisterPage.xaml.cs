using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SGEducationNigeriaMobile.Models;
using SGEducationNigeriaMobile.Services;

namespace SGEducationNigeriaMobile.Pages;

public partial class RegisterPage : ContentPage
{
    
    private readonly ApiService _apiService;

    public RegisterPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private void OnLoginTapped(object sender, EventArgs e)
    {
        
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var student = new
        {
            firstName = FirstNameEntry.Text,
            surname = SurnameEntry.Text,
            email = EmailEntry.Text,
            phoneNumber = PhoneEntry.Text,
            password = PasswordEntry.Text,

            address = "",
            dateOfBirth = "",
            preferredAcademicIntake = "",
            marritalStatus = "",
            happyToTravelFirst = false,
            yearOfLastAcademicStudies = 0,
            qualificationObtained = "",
            programOfStudy = "",
            grades = "",
            yearOfCompletion = 0,
            sponsor = 0,
            availableDeposit = 0,
            anyAgent = false,
            canYouStopAgent = false,
            anyVisaRefusal = false,
            anyBan = false,
            availabilityOfMaintenanceFunds = false,
            readyToProceedNow = false,
            totalArriveAbroadBudget = 0,
            areFundsAvailableNow = "",
            tryYourLuckWithChosenCountryOrNot = "",
            dateApplied = DateTime.UtcNow.ToString("o")
        };

        var response = await _apiService.RegisterStudent(student);

        if (response != null)
        {
            
            // ✅ STORE STUDENT ID
            SessionManager.StudentId = response.Id;

            await DisplayAlert("Success", "Account created!", "OK");

            var wizard = App.Current.Handler.MauiContext.Services
                .GetService<RegistrationWizardPage>();

            await Navigation.PushAsync(wizard);
            
            
            // await DisplayAlert("Success", "Account created! Please login.", "OK");
            //
            // var login = App.Current.Handler.MauiContext.Services
            //     .GetService<LoginPage>();
            //
            // await Navigation.PushAsync(login);
            
        }
        else
        {
            await DisplayAlert("Error", "Registration failed", "OK");
        }
    }
}