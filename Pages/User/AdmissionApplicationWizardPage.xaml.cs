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
        
        object studentData = step switch
        {
            0 => new
            {
                address = EntryAddress.Text,
                dateOfBirth = Convert.ToDateTime( PickerDob.Date.ToString()).ToString("yyyy-MM-dd"),
                marritalStatus = PickerMarital.SelectedItem?.ToString(),
                happyToTraverlFirst = PickerHappyToTravelFirst.SelectedItem?.ToString()
            },
        
            1 => new
            {
                // PreferredAcademicIntake = PickerPreferedAcademicIntake.SelectedItem?.ToString(),
                // CountryOfStudy1 = PickerCountryOfStudy1.SelectedItem?.ToString(),
                // CountryOfStudyId1,
                // CountryOfStudy2 = PickerCountryOfStudy2.SelectedItem?.ToString(),
                // CountryOfStudyId2,
                // CountryOfStudy3 = PickerCountryOfStudy3.SelectedItem?.ToString(),
                // CountryOfStudyId3,
                
                
                PreferredAcademicIntake = PickerPreferedAcademicIntake.SelectedItem?.ToString(),
                CountryOfStudy1 = countryOfStudyId1,
                CountryOfStudy2 = countryOfStudyId2,
                CountryOfStudy3 = countryOfStudyId3,
                programOfStudy = EntryProgramOfStudy.Text,
                qualificationObtained = EntryQualification.Text,
                grades = EntryGrades.Text
                
            },
        
            2 => new
            {
                sponsor = EntrySponsor.Text,
                totalArriveAbroadBudget = EntryBudget.Text,
                availableDeposit = EntryDeposit.Text,
                availabilityOfMaintenanceFunds = SwitchFundsForMaintenance.IsToggled,
                areFundsAvailableNow = SwitchFundsAvailableNow.IsToggled
            },
        
            3 => new
            {
                anyAgent = SwitchAnyOtherAgent.IsToggled,
                canYouStopAgent = SwitchCanYouStopAgent.IsToggled,
                readyToProceedNow = SwitchReady.IsToggled,
                anyVisaRefusal = EntryVisaRefusal.Text,
                tryYourLuckWithChosenCountryOrNot = EntryTryYourLuck.Text
            },
        
            _ => null
        };
        
        if (studentData != null)
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
                        //deleting the record
                        // await _api.DeleteStudentCountryOfPreference(c.Id);

                    }


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
                        // StudentCountryOfPreference studentCountryOfPreferenceObj3 = new StudentCountryOfPreference();
                        // studentCountryOfPreferenceObj3.StudentId = SessionManager.StudentId;
                        // studentCountryOfPreferenceObj3.CountryId = countryOfStudyId3;
                        
                        var studentCountryOfPreferenceObj3 = new StudentCountryOfPreference
                        {
                            StudentId = SessionManager.StudentId,
                            CountryId = countryOfStudyId3
                        };


                        await _api.CreateStudentCountryOfPreference(studentCountryOfPreferenceObj3);
                    }
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