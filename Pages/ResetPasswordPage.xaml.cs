using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGEducationNigeriaMobile.Services;

namespace SGEducationNigeriaMobile.Pages;


public partial class ResetPasswordPage : ContentPage
{
    private readonly ApiService _api;
    private readonly string _email;  

    public ResetPasswordPage(ApiService api, string email)
    {
        InitializeComponent();
        _api = api;
        _email = email;
    }

    private async void OnResetPasswordClicked(object sender, EventArgs e)
    {
        var newPass = NewPasswordEntry.Text?.Trim();
        var confirmPass = ConfirmPasswordEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(confirmPass))
        {
            await DisplayAlert("Error", "Please fill all fields.", "OK");
            return;
        }

        if (newPass != confirmPass)
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        // var result = await _api.ResetPasswordAsync(_email, newPass);
        //
        // if (result)
        // {
        //     await DisplayAlert("Success", "Password reset successfully.", "OK");
        //     await Navigation.PopToRootAsync();
        // }
        // else
        // {
        //     await DisplayAlert("Error", "Failed to reset password.", "OK");
        // }
    }
}
