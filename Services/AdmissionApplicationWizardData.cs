namespace SGEducationNigeriaMobile.Services;

public class AdmissionApplicationWizardData
{
    // Step 0
    public string Address { get; set; }
    public string DateOfBirth { get; set; }
    
    public string Age { get; set; }
    public string MaritalStatus { get; set; }
    public string HappyToTravelFirst { get; set; }

    // Step 1
    public string PreferredAcademicIntake { get; set; }
    public int CountryOfStudy1 { get; set; }
    public int CountryOfStudy2 { get; set; }
    public int CountryOfStudy3 { get; set; }
    public string ProgramOfStudy { get; set; }
    public string QualificationObtained { get; set; }
    public string Grades { get; set; }

    // Step 2
    public string Sponsor { get; set; }
    public string TotalArriveAbroadBudget { get; set; }
    public string AvailableDeposit { get; set; }
    public bool AvailabilityOfMaintenanceFunds { get; set; }
    public bool AreFundsAvailableNow { get; set; }

    // Step 3
    public bool AnyAgent { get; set; }
    public bool CanYouStopAgent { get; set; }
    public bool ReadyToProceedNow { get; set; }
    public string AnyVisaRefusalOrBan { get; set; }
    public string TryYourLuckWithChosenCountryOrNot { get; set; }
}