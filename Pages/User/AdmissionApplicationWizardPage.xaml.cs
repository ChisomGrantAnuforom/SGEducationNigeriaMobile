using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SGEducationNigeriaMobile.Models;
using SGEducationNigeriaMobile.Services;

namespace SGEducationNigeriaMobile.Pages.User;

public partial class AdmissionApplicationWizardPage : ContentPage
{
    private readonly ApiService _api;
    private int step = 0;
    
    
    private bool _isBusy = false;



    private AdmissionApplicationWizardData wizardData = new AdmissionApplicationWizardData();

    public AdmissionApplicationWizardPage(ApiService api)
    {
        try
        {
            InitializeComponent();
            this.Title = "Application for Admission";
            _api = api;
            UpdateUI();
            LoadYears();
            Routing.RegisterRoute("DocumentUploadPage", typeof(DocumentUploadPage));
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }         
    
   
    private void SetBusy(bool value)
    {
        _isBusy = value;

        BusyIndicator.IsVisible = value;
        BusyIndicator.IsRunning = value;

        ButtonNext.IsEnabled = !value;
        ButtonBack.IsEnabled = !value;
    }

    
    
    public async Task LoadCountries()
    {
        var countriesList = await _api.GetCountries();

        // PickerCountryOfStudy1.Items.Clear();
        // PickerCountryOfStudy2.Items.Clear();
        // PickerCountryOfStudy3.Items.Clear(); 

        if (PickerCountryOfStudy1.Items.Count == 0 && PickerCountryOfStudy2.Items.Count == 0 &&
            PickerCountryOfStudy3.Items.Count == 0)
        {
            PickerCountryOfStudy1.Items.Add("Select Country 1...");
            PickerCountryOfStudy2.Items.Add("Select Country 2...");
            PickerCountryOfStudy3.Items.Add("Select Country 3...");

            foreach (var c in countriesList)
            {
                PickerCountryOfStudy1.Items.Add(c.CountryName);
                PickerCountryOfStudy2.Items.Add(c.CountryName);
                PickerCountryOfStudy3.Items.Add(c.CountryName);
            }

            PickerCountryOfStudy1.SelectedIndex = 0;
            PickerCountryOfStudy2.SelectedIndex = 0;
            PickerCountryOfStudy3.SelectedIndex = 0;
        }

    }


    
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await LoadCountries();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error loading countries", ex.Message, "OK");
        }
    }

    
    public void LoadYears()
    {
        PickerYearOfLastAcademicStudies.Items.Add("Select Year...");
        
        int currentYear = DateTime.Now.Year;
        for (int year = currentYear; year >= 1980; year--)
        {
            PickerYearOfLastAcademicStudies.Items.Add(year.ToString());
        }

        PickerYearOfLastAcademicStudies.Title = "Select Year...";

    }
    
    private void UpdateUI()
    {
        Step1Layout.IsVisible = step == 0;
        Step2Layout.IsVisible = step == 1;
        Step3Layout.IsVisible = step == 2;
        Step4Layout.IsVisible = step == 3;

        StepTitle.Text = $"Step {step + 1} of 4";
        ProgressBar.Progress = (step + 1) / 4.0;
    }

    private async void OnNextClicked(object sender, EventArgs e)
    {
        
        if (_isBusy) return;
        
        try
        {
            
            SetBusy(true);
            
            // if(!ValidateStep(step))
            //     return;

            await SaveStep();

            if (step < 3)
            {
                step++;
                UpdateUI();
            }
            else
            {
                await Shell.Current.GoToAsync("DocumentUploadPage");
                // await DisplayAlert("Done", "Registration complete!", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
              await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        if (_isBusy) return;
        
        if (step > 0)
        {
            step--;
            UpdateUI();
        }
    }

    private async Task SaveStep()
    {

        SetBusy(true);
        
        int countryOfStudyId1 = 0;
        int countryOfStudyId2 = 0;
        int countryOfStudyId3 = 0;

        // STEP 1: Capture country IDs BEFORE creating the anonymous object
        if (step == 1)
        {
            if (PickerCountryOfStudy1.SelectedIndex > 0)
                countryOfStudyId1 = (await _api.GetCountryByCountryName(PickerCountryOfStudy1.SelectedItem.ToString())).Id;

            if (PickerCountryOfStudy2.SelectedIndex > 0)
                countryOfStudyId2 = (await _api.GetCountryByCountryName(PickerCountryOfStudy2.SelectedItem.ToString())).Id;

            if (PickerCountryOfStudy3.SelectedIndex > 0)
                countryOfStudyId3 = (await _api.GetCountryByCountryName(PickerCountryOfStudy3.SelectedItem.ToString())).Id;
        }
        
        
        switch (step)
        {
            case 0:
                wizardData.Address  = EntryAddress.Text;
                wizardData.DateOfBirth = Convert.ToDateTime( PickerDob.Date.ToString()).ToString("yyyy-MM-dd");
                wizardData.MaritalStatus = PickerMarital.SelectedItem?.ToString();
                wizardData.HappyToTravelFirst = PickerHappyToTravelFirst.SelectedItem?.ToString();
                break;
            
            case 1:
                wizardData.PreferredAcademicIntake = PickerPreferedAcademicIntake.SelectedItem?.ToString();
                wizardData.CountryOfStudy1 = countryOfStudyId1;
                wizardData.CountryOfStudy2 = countryOfStudyId2;
                wizardData.CountryOfStudy3 = countryOfStudyId3;
                wizardData.ProgramOfStudy = EntryProgramOfStudy.Text;
                wizardData.QualificationObtained = EntryQualification.Text;
                wizardData.Grades = EntryGrades.Text;
                
                break;

            case 2:
                wizardData.Sponsor = "0";
                wizardData.TotalArriveAbroadBudget = EntryBudget.Text;
                wizardData.AvailableDeposit = EntryDeposit.Text;
                wizardData.AvailabilityOfMaintenanceFunds = SwitchFundsForMaintenance.IsToggled;
                wizardData.AreFundsAvailableNow = SwitchFundsAvailableNow.IsToggled;
                break;

            case 3:
                wizardData.AnyAgent = SwitchAnyOtherAgent.IsToggled;
                wizardData.CanYouStopAgent = SwitchCanYouStopAgent.IsToggled;
                wizardData.ReadyToProceedNow = SwitchReady.IsToggled;
                wizardData.AnyVisaRefusalOrBan = EntryVisaRefusal.Text;
                wizardData.TryYourLuckWithChosenCountryOrNot = EntryTryYourLuck.Text;
                break;

            default:
                break;
        }
        
        
        
        var studentData = new
        {
            firstName = SessionManager.FirstName,
            surname = SessionManager.Surname,
            email = SessionManager.Email,
            phoneNumber = SessionManager.PhoneNumber,
            address = wizardData.Address,
            dateOfBirth = wizardData.DateOfBirth,
            marritalStatus = wizardData.MaritalStatus,
            happyToTravelFirst = wizardData.HappyToTravelFirst == "Yes",
            preferredAcademicIntake = wizardData.PreferredAcademicIntake,
            programOfStudy = wizardData.ProgramOfStudy,
            qualificationObtained = wizardData.QualificationObtained,
            grades = wizardData.Grades,
            sponsor = Convert.ToInt32(wizardData.Sponsor),
            totalArriveAbroadBudget = Convert.ToDecimal(wizardData.TotalArriveAbroadBudget),
            availableDeposit = Convert.ToDecimal(wizardData.AvailableDeposit),
            availabilityOfMaintenanceFunds = wizardData.AvailabilityOfMaintenanceFunds,
            areFundsAvailableNow = wizardData.AreFundsAvailableNow ? "Yes" : "No",
            anyAgent = wizardData.AnyAgent,
            canYouStopAgent = wizardData.CanYouStopAgent,
            readyToProceedNow = wizardData.ReadyToProceedNow,
            anyVisaRefusalOrBan = wizardData.AnyVisaRefusalOrBan ?? "",
            tryYourLuckWithChosenCountryOrNot = wizardData.TryYourLuckWithChosenCountryOrNot,
            yearOfLastAcademicStudies = Convert.ToInt32(PickerYearOfLastAcademicStudies.SelectedItem),
            yearOfCompletion = Convert.ToInt32(PickerYearOfLastAcademicStudies.SelectedItem),
            countryOfStudy1 = countryOfStudyId1,
            countryOfStudy2 = countryOfStudyId2,
            countryOfStudy3 = countryOfStudyId3,
            age = CalculateAge(wizardData.DateOfBirth),
            dateApplied = "2026-03-11T05:24:04.905Z",
            password = "admin",
            onboardingComplete = true
        };

        
        
        // var studentData = new
        // {
        //     firstName = SessionManager.FirstName,
        //     surname = SessionManager.Surname,
        //     email = SessionManager.Email,
        //     phoneNumber = SessionManager.PhoneNumber,
        //     address = wizardData.Address,
        //     dateOfBirth = wizardData.DateOfBirth,
        //     marritalStatus = wizardData.MaritalStatus,
        //     happyToTravelFirst = wizardData.HappyToTravelFirst == "Yes" ? true : false,
        //     preferredAcademicIntake = wizardData.PreferredAcademicIntake,
        //     programOfStudy = wizardData.ProgramOfStudy,
        //     qualificationObtained = wizardData.QualificationObtained,
        //     grades = wizardData.Grades,
        //     sponsor = String.IsNullOrWhiteSpace( wizardData.Sponsor) ? 0 : Convert.ToInt32( wizardData.Sponsor ),
        //     totalArriveAbroadBudget = String.IsNullOrWhiteSpace( wizardData.TotalArriveAbroadBudget) ? 0 : Convert.ToDecimal( wizardData.TotalArriveAbroadBudget),
        //     availableDeposit = String.IsNullOrWhiteSpace( wizardData.AvailableDeposit) ? 0 : Convert.ToDecimal(wizardData.AvailableDeposit),
        //     availabilityOfMaintenanceFunds = wizardData.AvailabilityOfMaintenanceFunds,
        //     areFundsAvailableNow = wizardData.AreFundsAvailableNow,
        //     anyAgent = wizardData.AnyAgent,
        //     canYouStopAgent = wizardData.CanYouStopAgent,
        //     readyToProceedNow = wizardData.ReadyToProceedNow,
        //     anyVisaRefusalOrBan = wizardData.AnyVisaRefusalOrBan,
        //     tryYourLuckWithChosenCountryOrNot = wizardData.TryYourLuckWithChosenCountryOrNot,
        //     yearOfLastAcademicStudies = 0,  //this needs to be validated from UI
        //     yearOfCompletion = 0, //this needs to be validated from UI
        //     dateApplied = "2026-03-11T05:24:04.905Z",
        //     password = "admin",
        //     onboardingComplete = true
        //     
        //
        // };

        
        // object studentData = step switch
        // {
        //     0 => new
        //     {
        //          wizardData.Address = EntryAddress.Text,
        //         dateOfBirth = Convert.ToDateTime( PickerDob.Date.ToString()).ToString("yyyy-MM-dd"),
        //         marritalStatus = PickerMarital.SelectedItem?.ToString(),
        //         happyToTraverlFirst = PickerHappyToTravelFirst.SelectedItem?.ToString()
        //     },
        //
        //     1 => new
        //     {
        //         // PreferredAcademicIntake = PickerPreferedAcademicIntake.SelectedItem?.ToString(),
        //         // CountryOfStudy1 = PickerCountryOfStudy1.SelectedItem?.ToString(),
        //         // CountryOfStudyId1,
        //         // CountryOfStudy2 = PickerCountryOfStudy2.SelectedItem?.ToString(),
        //         // CountryOfStudyId2,
        //         // CountryOfStudy3 = PickerCountryOfStudy3.SelectedItem?.ToString(),
        //         // CountryOfStudyId3,
        //         
        //         
        //         PreferredAcademicIntake = PickerPreferedAcademicIntake.SelectedItem?.ToString(),
        //         CountryOfStudy1 = countryOfStudyId1,
        //         CountryOfStudy2 = countryOfStudyId2,
        //         CountryOfStudy3 = countryOfStudyId3,
        //         programOfStudy = EntryProgramOfStudy.Text,
        //         qualificationObtained = EntryQualification.Text,
        //         grades = EntryGrades.Text
        //         
        //     },
        //
        //     2 => new
        //     {
        //         sponsor = EntrySponsor.Text,
        //         totalArriveAbroadBudget = EntryBudget.Text,
        //         availableDeposit = EntryDeposit.Text,
        //         availabilityOfMaintenanceFunds = SwitchFundsForMaintenance.IsToggled,
        //         areFundsAvailableNow = SwitchFundsAvailableNow.IsToggled
        //     },
        //
        //     3 => new
        //     {
        //         anyAgent = SwitchAnyOtherAgent.IsToggled,
        //         canYouStopAgent = SwitchCanYouStopAgent.IsToggled,
        //         readyToProceedNow = SwitchReady.IsToggled,
        //         anyVisaRefusal = EntryVisaRefusal.Text,
        //         tryYourLuckWithChosenCountryOrNot = EntryTryYourLuck.Text
        //     },
        //
        //     _ => null
        // };


        // int count = CountProperties(studentData);
        //
        // int count2 = count;
        
        if (studentData != null && step == 3)
        {
            await _api.UpdateStudent(SessionManager.StudentId, studentData);
            try
            {
                //saving student country of preference data
                IEnumerable<StudentCountryOfPreference> studentStudyCountry =  await _api.GetStudentCountryOfPreferenceByStudentId(SessionManager.StudentId);
                if (studentStudyCountry.Count() > 0) //if the student has existing records for country of preference
                {
                    foreach (var c in studentStudyCountry )
                    {
                        
                        //checking if any of the countries matches the selected countries
                        if (c.CountryId != countryOfStudyId1 && c.CountryId != countryOfStudyId2 && c.CountryId != countryOfStudyId3)
                        {

                            if (countryOfStudyId1 > 0)
                            {

                                var studentCountryOfPreferenceObj1 = new StudentCountryOfPreference()
                                {
                                    StudentId = SessionManager.StudentId,
                                    CountryId = countryOfStudyId1
                                };

                                await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj1);

                            }


                            if (countryOfStudyId2 > 0)
                            {

                                var studentCountryOfPreferenceObj2 = new StudentCountryOfPreference
                                {
                                    StudentId = SessionManager.StudentId,
                                    CountryId = countryOfStudyId2
                                };

                                await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj2);

                            }


                            if (countryOfStudyId3 > 0)
                            {

                                var studentCountryOfPreferenceObj3 = new StudentCountryOfPreference
                                {
                                    StudentId = SessionManager.StudentId,
                                    CountryId = countryOfStudyId3
                                };


                                await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj3);

                            }

                        }
                        // else
                        // {
                        //     //deleting the record
                        //     await _api.DeleteStudentCountryOfPreference(c.Id);
                        //     
                        // }
                     
                  
                        // if (c.CountryId == countryOfStudyId2 )//checking country two
                        // {
                        //     //deleting the record
                        //     await _api.DeleteStudentCountryOfPreference(c.Id);
                        // }
                        //
                        //
                        // if (c.CountryId == countryOfStudyId3 )//checking country three
                        // {
                        //     //deleting the record
                        //     await _api.DeleteStudentCountryOfPreference(c.Id);
                        // }

                    }

 

                    // if (countryOfStudyId1 > 0)
                    // {
                    //     var studentCountryOfPreferenceObj1 = new StudentCountryOfPreference()
                    //     {
                    //         StudentId = SessionManager.StudentId,
                    //         CountryId = countryOfStudyId1
                    //     };
                    //     
                    //     await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj1);
                    // }
                    //
                    // if (countryOfStudyId2 > 0)
                    // {
                    //     var studentCountryOfPreferenceObj2 = new StudentCountryOfPreference
                    //     {
                    //         StudentId = SessionManager.StudentId,
                    //         CountryId = countryOfStudyId2
                    //     };
                    //
                    //     await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj2);
                    // }
                    //
                    //
                    // if (countryOfStudyId3 > 0)
                    // {
                    //     
                    //     var studentCountryOfPreferenceObj3 = new StudentCountryOfPreference
                    //     {
                    //         StudentId = SessionManager.StudentId,
                    //         CountryId = countryOfStudyId3
                    //     };
                    //
                    //
                    //     await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj3);
                    // }
                }
                else  //f the student does not have existing records for country of preference
                {
                    if (countryOfStudyId1 > 0)
                    {
                        var studentCountryOfPreferenceObj1 = new StudentCountryOfPreference
                        {
                            StudentId = SessionManager.StudentId,
                            CountryId = countryOfStudyId1
                        };

                        await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj1);
                    }

                    if (countryOfStudyId2 > 0)
                    {
                        var studentCountryOfPreferenceObj2 = new StudentCountryOfPreference()
                        {
                            StudentId = SessionManager.StudentId,
                            CountryId = countryOfStudyId2
                        };
                        
                        await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj2);
                    }
                    
                    
                    if (countryOfStudyId3 > 0)
                    {
                        var studentCountryOfPreferenceObj3 = new StudentCountryOfPreference()
                        {
                            StudentId = SessionManager.StudentId,
                            CountryId = countryOfStudyId3
                        };
                        await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj3);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Error has occured. "+ex.Message, "OK");
                Console.WriteLine("ERRRRR ##########"+ex.Message);
            }
        }
    }
    
    
    
    public int CalculateAge(string dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(dateOfBirth))
            return 0;

        // Parse the date safely
        DateTime dob = DateTime.Parse(dateOfBirth);

        int age = DateTime.Today.Year - dob.Year;

        // Adjust if birthday hasn't happened yet this year
        if (dob.Date > DateTime.Today.AddYears(-age)) 
            age--;

        return age;
    }
    
    public static int CountProperties(object obj)
    {
        if (obj == null)
            return 0;

        return obj.GetType().GetProperties().Length;
    }
    

    private bool ValidateStep(int step)
    {
        switch (step)
        {
            case 0:
                if (string.IsNullOrWhiteSpace(EntryAddress.Text))
                {
                    DisplayAlert("Validation", "Please enter your address.", "OK");
                    return false;
                }

                if (PickerDob.Date == default)
                {
                    DisplayAlert("Validation", "Please select your date of birth.", "OK");
                    return false;
                }

                if (PickerMarital.SelectedItem == null)
                {
                    DisplayAlert("Validation", "Please select your marital status.", "OK");
                    return false;
                }

                break;

            case 1:
                if (PickerPreferedAcademicIntake.SelectedItem == null)
                {
                    DisplayAlert("Validation", "Please select your preferred academic intake.", "OK");
                    return false;
                }

                if (PickerYearOfLastAcademicStudies.SelectedIndex <= 0)
                {
                    DisplayAlert("Validation", "Please select the year you finished your studies.", "OK");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(EntryProgramOfStudy.Text))
                {
                    DisplayAlert("Validation", "Please enter your program of study.", "OK");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(EntryQualification.Text))
                {
                    DisplayAlert("Validation", "Please enter your qualification.", "OK");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(EntryGrades.Text))
                {
                    DisplayAlert("Validation", "Please enter your grades.", "OK");
                    return false;
                }

                break;

            case 2:
                if (string.IsNullOrWhiteSpace(EntryBudget.Text))
                {
                    DisplayAlert("Validation", "Please enter your budget.", "OK");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(EntryDeposit.Text))
                {
                    DisplayAlert("Validation", "Please enter your deposit amount.", "OK");
                    return false;
                }

                break;

            case 3:
                if (!SwitchReady.IsToggled)
                {
                    DisplayAlert("Validation", "Please confirm you are ready to proceed.", "OK");
                    return false;
                }

                break;
        }

        return true;
    }



}