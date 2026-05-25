using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGEducationNigeriaMobile.Services;

namespace SGEducationNigeriaMobile.Pages.User;

public partial class DashboardPage : ContentPage
{
    
    private readonly ApiService _api;
    
    public DashboardPage(ApiService api)
    {
        
        try
        {
            InitializeComponent();
            
            _api = api;

            LabelStudentName.Text = SessionManager.StudentName;
            
            ValidateStudentStatus();

            Routing.RegisterRoute("AdmissionApplicationWizardPage", typeof(AdmissionApplicationWizardPage));
            
            Routing.RegisterRoute("DocumentUploadPage", typeof(DocumentUploadPage));
        }
        catch (Exception ex)
        {
            Console.WriteLine("RRRRRRRRR::::: "+ex.Message);
        }
    }
    

    private async void ValidateStudentStatus()
    {
        var studentData = await _api.GetStudentByStudentId(SessionManager.StudentId);
        if (studentData != null)
        {
            LabelApplyForAdmissionInfo.Text = "View Your Application";
        }
        
        var studentDocumentData = await _api.GetStudentDocuments(SessionManager.StudentId);
        if (studentDocumentData != null)
        {
            LabelUploadDocsInfo.Text  = "View Your Upload Documents";
        }
    }

    private async void  ButtonCompleteRegistration_OnClicked(object? sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("AdmissionApplicationWizardPage");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
       
    
    }

    private async void ButtonUploadDocumentsTapGestureRecognizer_OnTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("DocumentUploadPage");
            // await DisplayAlert("Registration Complete", "Registration Complete", "OK");
            Console.WriteLine("#######Tapped.....");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error/Exception", ex.Message, "OK");
            Console.WriteLine(ex.Message);
        }
    }

    private async void ButtonAppplyTapGestureRecognizer_OnTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            
            await Shell.Current.GoToAsync("AdmissionApplicationWizardPage");
        }
        catch (Exception ex)
        {
           await  DisplayAlert("Error/Exception", ex.Message, "OK");
        }
    }
}