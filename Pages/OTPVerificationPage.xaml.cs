using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGEducationNigeriaMobile.Services;

namespace SGEducationNigeriaMobile.Pages;


public partial class OTPVerificationPage : ContentPage
{
    private readonly ApiService _api;
    private readonly string _email; 

    public OTPVerificationPage(ApiService api, string email)
    {
        InitializeComponent();
        _api = api;
        _email = email;
    }

    private async void OnVerifyOtpClicked(object sender, EventArgs e)
    {
        var otp = OtpEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(otp))
        {
            await DisplayAlert("Error", "Please enter the OTP.", "OK");
            return;
        }

        // var result = await _api.VerifyOtpAsync(_email, otp);
        //
        // if (result)
        // {
        //     await Navigation.PushAsync(new ResetPasswordPage(_api, _email));
        // }
        // else
        // {
        //     await DisplayAlert("Error", "Invalid OTP. Try again.", "OK");
        // }
    }
}
