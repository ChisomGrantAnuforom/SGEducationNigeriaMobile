using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGEducationNigeriaMobile.Models;
using SGEducationNigeriaMobile.Services;

namespace SGEducationNigeriaMobile.Pages.User;

public partial class ProfilePage : ContentPage
{
    
    private readonly ApiService _api;
    private Student studentObj;
    
    public ProfilePage(ApiService api)
    {
        InitializeComponent();
        
        _api = api;
        
        LoadStudentAdmissionData(SessionManager.StudentId);
        
    }

    
    
    private async void LoadStudentAdmissionData(int studentId)
    {
        studentObj = await _api.GetStudentByStudentId(studentId);

        if (studentObj != null)
        {
            //load to controls
            //step one controls
            EntryFirstName.Text= studentObj.FirstName;
            EntrySurname.Text = studentObj.Surname;
            EntryEmail.Text = studentObj.Email;
            EntryPhoneNumber.Text = studentObj.PhoneNumber;
      

        }
    }
    
    private async void OnSaveProfileClicked(object sender, EventArgs e)
    {
        

        var studentData = new
        {
            firstName = EntryFirstName.Text,
            surname = EntrySurname.Text,
            email = EntryEmail.Text,
            phoneNumber = EntryPhoneNumber.Text,
            
            
            // address = wizardData.Address,
            // dateOfBirth = wizardData.DateOfBirth,
            // marritalStatus = wizardData.MaritalStatus,
            // happyToTravelFirst = wizardData.HappyToTravelFirst,
            // preferredAcademicIntake = wizardData.PreferredAcademicIntake,
            // programOfStudy = wizardData.ProgramOfStudy,
            // qualificationObtained = wizardData.QualificationObtained,
            // grades = wizardData.Grades,
            // sponsor = Convert.ToInt32(wizardData.Sponsor),
            // totalArriveAbroadBudget = Convert.ToDecimal(wizardData.TotalArriveAbroadBudget),
            // availableDeposit = Convert.ToDecimal(wizardData.AvailableDeposit),
            // availabilityOfMaintenanceFunds = wizardData.AvailabilityOfMaintenanceFunds,
            // areFundsAvailableNow = wizardData.AreFundsAvailableNow,
            // anyAgent = wizardData.AnyAgent,
            // canYouStopAgent = wizardData.CanYouStopAgent,
            // readyToProceedNow = wizardData.ReadyToProceedNow,
            // anyVisaRefusalOrBan = wizardData.AnyVisaRefusalOrBan ?? "",
            // tryYourLuckWithChosenCountryOrNot = wizardData.TryYourLuckWithChosenCountryOrNot,
            // yearOfLastAcademicStudies = Convert.ToInt32(PickerYearOfLastAcademicStudies.SelectedItem),
            // yearOfCompletion = Convert.ToInt32(PickerYearOfLastAcademicStudies.SelectedItem),
            // countryOfStudy1 = countryOfStudyId1,
            // countryOfStudy2 = countryOfStudyId2,
            // countryOfStudy3 = countryOfStudyId3,
            // age = CalculateAge(wizardData.DateOfBirth),
            // dateApplied = "2026-03-11T05:24:04.905Z",
            // password = "admin",
            // onboardingComplete = true
        };


        if (studentData != null )
        {

            try
            {

                await _api.UpdateStudent(SessionManager.StudentId, studentData);
                
                await DisplayAlert("Success", "Your profile has been updated.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        
    }
}
