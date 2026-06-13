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
    private int _countdown = 30;
    private bool _timerRunning = false;

    public OTPVerificationPage(ApiService api, string email)
    {
        InitializeComponent();
        _api = api;
        _email = email;

        StartCountdown();
    }

    private async void OnVerifyOtpClicked(object sender, EventArgs e)
    {
        var otp = OtpEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(otp))
        {
            await DisplayAlert("Error", "Please enter the OTP.", "OK");
            return;
        }

        var result = await _api.VerifyOtpAsync(_email, otp);

        if (result)
        {
            await Navigation.PushAsync(new ResetPasswordPage(_api, _email));
        }
        else
        {
            await DisplayAlert("Error", "Invalid OTP. Try again.", "OK");
        }
    }

    private void StartCountdown()
    {
        _countdown = 30;
        ResendButton.IsEnabled = false;
        ResendButton.BackgroundColor = Color.FromArgb("#D1D5DB");
        ResendButton.TextColor = Color.FromArgb("#6B7280");

        CountdownLabel.Text = $"Resend code in {_countdown}s";

        if (_timerRunning) return;
        _timerRunning = true;

        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            _countdown--;

            if (_countdown > 0)
            {
                CountdownLabel.Text = $"Resend code in {_countdown}s";
                return true;
            }

            // Enable resend
            CountdownLabel.Text = "You can resend the code now";
            ResendButton.IsEnabled = true;
            ResendButton.BackgroundColor = Color.FromArgb("#4F46E5");
            ResendButton.TextColor = Colors.White;

            _timerRunning = false;
            return false;
        });
    }

    private async void OnResendOtpClicked(object sender, EventArgs e)
    {
        var result = await _api.SendPasswordResetAsync(_email);

        if (result)
        {
            await DisplayAlert("Success", "A new OTP has been sent.", "OK");
            StartCountdown();
        }
        else
        {
            await DisplayAlert("Error", "Failed to resend OTP.", "OK");
        }
    }
}
