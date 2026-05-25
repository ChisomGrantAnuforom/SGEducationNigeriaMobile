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
            LoadStudentAdmissionData(SessionManager.StudentId);
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
            
            if(!ValidateStep(step))
                return;

            await SaveStep();

            if (step < 3)
            {
                step++;
                UpdateUI();
            }
            else
            {
                await Shell.Current.GoToAsync("DocumentUploadPage");
                Navigation.RemovePage(this);
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

    
    int countryOfStudyId1 = 0;
    int countryOfStudyId2 = 0;
    int countryOfStudyId3 = 0;

    
    private async Task SaveStep()
    {

        SetBusy(true);

     

   
        // STEP 1: Capture country IDs BEFORE creating the anonymous object
        if (step == 1)
        {
            if (PickerCountryOfStudy1.SelectedIndex > 0)
                countryOfStudyId1 = (await _api.GetCountryByCountryName(PickerCountryOfStudy1.SelectedItem.ToString()))
                    .Id;

            if (PickerCountryOfStudy2.SelectedIndex > 0)
                countryOfStudyId2 = (await _api.GetCountryByCountryName(PickerCountryOfStudy2.SelectedItem.ToString()))
                    .Id;

            if (PickerCountryOfStudy3.SelectedIndex > 0)
                countryOfStudyId3 = (await _api.GetCountryByCountryName(PickerCountryOfStudy3.SelectedItem.ToString()))
                    .Id;
        }


        switch (step)
        {
            case 0:
                wizardData.Address = EntryAddress.Text;
                wizardData.DateOfBirth = Convert.ToDateTime(PickerDob.Date.ToString()).ToString("yyyy-MM-dd");
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
                wizardData.AvailabilityOfMaintenanceFunds = PickerFundsForMaintenance.SelectedItem?.ToString();
                wizardData.AreFundsAvailableNow = PickerFundsAvailableNow.SelectedItem?.ToString();
                break;


            case 3:
                wizardData.AnyAgent = PickerAnyOtherAgent.SelectedItem.ToString();
                wizardData.CanYouStopAgent = PickerCanYouStopAgent.SelectedItem.ToString();
                wizardData.ReadyToProceedNow = PickerReady.SelectedItem.ToString();
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
            happyToTravelFirst = wizardData.HappyToTravelFirst,
            preferredAcademicIntake = wizardData.PreferredAcademicIntake,
            programOfStudy = wizardData.ProgramOfStudy,
            qualificationObtained = wizardData.QualificationObtained,
            grades = wizardData.Grades,
            sponsor = Convert.ToInt32(wizardData.Sponsor),
            totalArriveAbroadBudget = Convert.ToDecimal(wizardData.TotalArriveAbroadBudget),
            availableDeposit = Convert.ToDecimal(wizardData.AvailableDeposit),
            availabilityOfMaintenanceFunds = wizardData.AvailabilityOfMaintenanceFunds,
            areFundsAvailableNow = wizardData.AreFundsAvailableNow,
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




        if (studentData != null && step == 3)
        {

            try
            {

                await _api.UpdateStudent(SessionManager.StudentId, studentData);

                //saving student country of preference data
                IEnumerable<StudentCountryOfPreference> studentStudyCountry =
                    await _api.GetStudentCountryOfPreferenceByStudentId(SessionManager.StudentId);
                if (studentStudyCountry.Count() > 0) //if the student has existing records for country of preference
                {
                    
                    foreach(var studentStudyCountryObj in studentStudyCountry)
                    {
                        await _api.DeleteStudentCountryOfPreference(studentStudyCountryObj.Id);
                    }
                    
                    //checking if student country of preference has already been saved
                    //country 1
                    // var matches = studentStudyCountry
                    //     .Where(x => x.CountryId == countryOfStudyId1);
                    //
                    // if (matches.Count() > 0)
                    // {
                    //     //delete the student country
                    //     await _api.DeleteStudentCountryOfPreferenceByCountryIdAndStudentId(countryOfStudyId1, SessionManager.StudentId);
                    // }
                    
                    var studentCountryOfPreferenceObj1 = new StudentCountryOfPreference()
                    {
                        StudentId = SessionManager.StudentId,
                        CountryId = countryOfStudyId1
                    };

                    await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj1);
                    
                    
                    
                    
                    
                    //country 2
                    // var matches2 = studentStudyCountry
                    //     .Where(x => x.CountryId == countryOfStudyId2);
                    //
                    // if (matches2.Count() > 0)
                    // {
                    //     //delete the student country
                    //     await _api.DeleteStudentCountryOfPreferenceByCountryIdAndStudentId(countryOfStudyId2, SessionManager.StudentId);
                    // }
                    
                    var studentCountryOfPreferenceObj2 = new StudentCountryOfPreference()
                    {
                        StudentId = SessionManager.StudentId,
                        CountryId = countryOfStudyId2
                    };

                    await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj2);
                    
                    
                    
                    
                    //country 3
                    // var matches3 = studentStudyCountry
                    //     .Where(x => x.CountryId == countryOfStudyId3);
                    //
                    // if (matches3.Count() > 0)
                    // {
                    //     //delete the student country
                    //     await _api.DeleteStudentCountryOfPreferenceByCountryIdAndStudentId(countryOfStudyId3, SessionManager.StudentId);
                    // }
                    
                    var studentCountryOfPreferenceObj3 = new StudentCountryOfPreference()
                    {
                        StudentId = SessionManager.StudentId,
                        CountryId = countryOfStudyId3
                    };

                    await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj3);
                    
                    
                    
                    
                    
                    
                    // foreach (var c in studentStudyCountry)
                    // {
                    //
                    //     //checking if any of the countries matches the selected countries
                    //     if (c.CountryId != countryOfStudyId1 && c.CountryId != countryOfStudyId2 &&
                    //         c.CountryId != countryOfStudyId3)
                    //     {
                    //
                    //         if (countryOfStudyId1 > 0)
                    //         {
                    //             
                    //             //checking if student country of preference has already been saved
                    //             var matches = studentStudyCountry
                    //                 .Where(x => x.CountryId == countryOfStudyId1);
                    //
                    //             if (matches.Count() > 0)
                    //             {
                    //                 //delete the student country
                    //                 await _api.DeleteStudentCountryOfPreference(countryOfStudyId1);
                    //             }
                    //
                    //
                    //             var studentCountryOfPreferenceObj1 = new StudentCountryOfPreference()
                    //             {
                    //                 StudentId = SessionManager.StudentId,
                    //                 CountryId = countryOfStudyId1
                    //             };
                    //
                    //             await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj1);
                    //
                    //         }
                    //
                    //
                    //         if (countryOfStudyId2 > 0)
                    //         {
                    //             
                    //             //checking if student country of preference has already been saved
                    //             var matches = studentStudyCountry
                    //                 .Where(x => x.CountryId == countryOfStudyId2);
                    //
                    //             if (matches.Count() > 0)
                    //             {
                    //                 //delete the student country
                    //                 await _api.DeleteStudentCountryOfPreference(countryOfStudyId2);
                    //             }
                    //             
                    //
                    //             var studentCountryOfPreferenceObj2 = new StudentCountryOfPreference
                    //             {
                    //                 StudentId = SessionManager.StudentId,
                    //                 CountryId = countryOfStudyId2
                    //             };
                    //
                    //             await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj2);
                    //
                    //         }
                    //
                    //
                    //         if (countryOfStudyId3 > 0)
                    //         {
                    //             
                    //             //checking if student country of preference has already been saved
                    //             var matches = studentStudyCountry
                    //                 .Where(x => x.CountryId == countryOfStudyId3);
                    //
                    //             if (matches.Count() > 0)
                    //             {
                    //                 //delete the student country
                    //                 await _api.DeleteStudentCountryOfPreference(countryOfStudyId3);
                    //             }
                    //
                    //             var studentCountryOfPreferenceObj3 = new StudentCountryOfPreference
                    //             {
                    //                 StudentId = SessionManager.StudentId,
                    //                 CountryId = countryOfStudyId3
                    //             };
                    //
                    //
                    //             await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj3);
                    //
                    //         }
                    //
                    //     }
                    //
                    //
                    // }

                }
                else //f the student does not have existing records for country of preference
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
                DisplayAlert("Error", "Error has occured. " + ex.Message, "OK");
                Console.WriteLine("ERRRRR ##########" + ex.Message);
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

                if (PickerCountryOfStudy1.SelectedIndex == 0)
                {
                    DisplayAlert("Validation", "Please select your first country of study.", "OK");
                    return false;
                }
                
                if (PickerCountryOfStudy2.SelectedIndex == 0)
                {
                    DisplayAlert("Validation", "Please select your second country of study.", "OK");
                    return false;
                }
                
                if (PickerCountryOfStudy3.SelectedIndex == 0)
                {
                    DisplayAlert("Validation", "Please select your third country of study.", "OK");
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

                if (string.IsNullOrWhiteSpace(EntrySponsor.Text))
                {
                    DisplayAlert("Validation", "Please enter your sponsor.", "OK");
                    return false;   
                }
                
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
                
                if (PickerFundsForMaintenance.SelectedItem == null)
                {
                    DisplayAlert("Validation", "Please select your maintenance funds status.", "OK");
                    return false;
                }
                
                
                if (PickerFundsAvailableNow.SelectedItem == null)
                {
                    DisplayAlert("Validation", "Please select your maintenance funds status.", "OK");
                    return false;
                }

                break;

            case 3:

                if (PickerAnyOtherAgent.SelectedItem == null)
                {
                    DisplayAlert("Validation", "Please select if you have any other agent.", "OK");
                    return false;
                }

                if (PickerCanYouStopAgent.SelectedItem == null)
                {
                    DisplayAlert("Validation", "Please select if you can stop agent.", "OK");
                    return false;
                }


                if (string.IsNullOrWhiteSpace(EntryVisaRefusal.Text))
                {
                    DisplayAlert("Validation", "Please enter your visa refusals, if any.", "OK");
                }
                
                if (PickerReady.SelectedIndex == 0)
                {
                    DisplayAlert("Validation", "Please confirm you are ready to proceed.", "OK");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(EntryTryYourLuck.Text))
                {
                    DisplayAlert("Validation", "Please enter if you prefer to try your luck with your chosen country...", "OK");
                }

                break;
        }

        return true;
    }



    private async void LoadStudentAdmissionData(int studentId)
    {
        var studentData = await _api.GetStudentByStudentId(studentId);

        if (studentData != null)
        {
            //load to controls
            //step one controls
            EntryAddress.Text= studentData.Address;
            PickerDob.Date = Convert.ToDateTime( studentData.DateOfBirth);
            PickerMarital.SelectedItem = studentData.MarritalStatus;
            PickerHappyToTravelFirst.SelectedItem = studentData.HappyToTravelFirst;
            
            //step two controls
            PickerPreferedAcademicIntake.SelectedItem = studentData.PreferredAcademicIntake;
            
            var studentCountryList = await _api.GetStudentCountryOfPreferenceByStudentId(studentId);
            if (studentCountryList != null)
            {
                if (studentCountryList.Count() == 1)
                {
                    var countryObj1 = await _api.GetCountry(studentCountryList.First().CountryId);
                    PickerCountryOfStudy1.SelectedItem = countryObj1.CountryName;
                    
                }
                else if (studentCountryList.Count() == 2)
                {
                    var countryObj1 = await _api.GetCountry(studentCountryList.First().CountryId);
                    PickerCountryOfStudy1.SelectedItem = countryObj1.CountryName;
                    
                    var countryObj2 = await _api.GetCountry(studentCountryList.ElementAt(1).CountryId);
                    PickerCountryOfStudy2.SelectedItem = countryObj2.CountryName;
                }
                else if (studentCountryList.Count() == 3)
                {
                    var countryObj1 = await _api.GetCountry(studentCountryList.First().CountryId);
                    PickerCountryOfStudy1.SelectedItem = countryObj1.CountryName;
                    
                    var countryObj2 = await _api.GetCountry(studentCountryList.ElementAt(1).CountryId);
                    PickerCountryOfStudy2.SelectedItem = countryObj2.CountryName;
                    
                    var countryObj3 = await _api.GetCountry(studentCountryList.ElementAt(2).CountryId);
                    PickerCountryOfStudy3.SelectedItem = countryObj3.CountryName;
                }
            }
            
            PickerYearOfLastAcademicStudies.SelectedItem = studentData.YearOfLastAcademicStudies;
            EntryProgramOfStudy.Text = studentData.ProgramOfStudy;
            EntryQualification.Text = studentData.QualificationObtained;
            EntryGrades.Text = studentData.Grades;
            
            
            //step three controls
            EntrySponsor.Text = studentData.Sponsor;
            EntryBudget.Text = studentData.TotalArriveAbroadBudget.ToString();
            EntryDeposit.Text = studentData.AvailableDeposit.ToString();
            PickerFundsForMaintenance.SelectedItem = studentData.AvailabilityOfMaintenanceFunds;
            PickerFundsAvailableNow.SelectedItem = studentData.AreFundsAvailableNow;
            
            
            //step four controls
            PickerAnyOtherAgent.SelectedItem = studentData.AnyAgent;
            PickerCanYouStopAgent.SelectedItem = studentData.CanYouStopAgent;
            EntryVisaRefusal.Text = studentData.AnyVisaRefusalOrBan;
            PickerReady.SelectedItem = studentData.ReadyToProceedNow;
            EntryTryYourLuck.Text = studentData.TryYourLuckWithChosenCountryOrNot;





        }
    }



}