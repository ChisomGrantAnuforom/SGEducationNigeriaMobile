using Microsoft.Extensions.DependencyInjection;

namespace SGEducationNigeriaMobile;

public partial class App : Application
{
    public App()
    {
        try
        {
            InitializeComponent();
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error::: " + ex.Message);

        }

    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}