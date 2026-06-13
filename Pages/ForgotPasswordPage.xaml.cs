using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGEducationNigeriaMobile.Services;

namespace SGEducationNigeriaMobile.Pages;


public partial class ForgotPasswordPage : ContentPage
{
    private readonly ApiService _api; 

    public ForgotPasswordPage(ApiService api)
    {
        InitializeComponent();
        _api = api;
    }

    private async void OnSendResetClicked(object sender, EventArgs e)
    {

        try
        {

            var email = EmailEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Error", "Please enter your email.", "OK");
                return;
            }

            var result = await _api.SendPasswordResetAsync(email);

            if (result)
            {
                await DisplayAlert("Success", "A reset code has been sent to your email.", "OK");
                await Navigation.PushAsync(new OTPVerificationPage(_api, email));
            }
            else
            {
                await DisplayAlert("Error", "Unable to send reset code. Try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
