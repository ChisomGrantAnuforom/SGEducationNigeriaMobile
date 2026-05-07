using SGEducationNigeriaMobile.Pages.User;

namespace SGEducationNigeriaMobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        // Routing.RegisterRoute("DashboardPage", typeof(DashboardPage));
    }
    
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Remove("StudentId");

        await Shell.Current.GoToAsync("//LoginPage");
    }
}