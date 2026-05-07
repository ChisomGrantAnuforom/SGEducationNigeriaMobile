using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGEducationNigeriaMobile.Services;

namespace SGEducationNigeriaMobile.Pages.User;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        this.Title = SessionManager.StudentName; 
    }
}