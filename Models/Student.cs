namespace SGEducationNigeriaMobile.Models;

public class Student
{
    public int Id { get; set; }

    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public int Age { get; set; }
    
    
    public string Address { get; set; }
    public string DateOfBirth { get; set; }
    public string PreferredAcademicIntake { get; set; }
    public string MarritalStatus { get; set; }
    public bool HappyToTravelFirst { get; set; }
    public int YearOfLastAcademicStudies { get; set; }

    public string QualificationObtained { get; set; }

    public string ProgramOfStudy { get; set; }

    public string Grades { get; set; }
         
    public int YearOfCompletion { get; set; }

    public int Sponsor { get; set; }

    public decimal AvailableDeposit { get; set; }

    public bool AnyAgent { get; set; }

    public bool CanYouStopAgent { get; set; }

    public string AnyVisaRefusalOrBan { get; set; }

    public bool AvailabilityOfMaintenanceFunds { get; set; }

    public bool ReadyToProceedNow { get; set; }

    public decimal TotalArriveAbroadBudget { get; set; }

    public string AreFundsAvailableNow { get; set; }

    public string TryYourLuckWithChosenCountryOrNot { get; set; }

    public string DateApplied { get; set; }
    
    public bool OnboardingComplete { get; set; } = false;
}