using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGEducationNigeriaMobile.Pages.User;
using SGEducationNigeriaMobile.Services;

namespace SGEducationNigeriaMobile;

public partial class UserShell : Shell
{
    public UserShell()
    {
        InitializeComponent();
     
    }
    
    
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Remove("StudentId");

        await Shell.Current.GoToAsync("//LoginPage");
    }
}