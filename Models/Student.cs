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
    public string HappyToTravelFirst { get; set; }
    public int YearOfLastAcademicStudies { get; set; }

    public string QualificationObtained { get; set; }

    public string ProgramOfStudy { get; set; }

    public string Grades { get; set; }
         
    public int YearOfCompletion { get; set; }

    public string Sponsor { get; set; }

    public decimal AvailableDeposit { get; set; }

    public string AnyAgent { get; set; }

    public string CanYouStopAgent { get; set; }

    public string AnyVisaRefusalOrBan { get; set; }

    public string AvailabilityOfMaintenanceFunds { get; set; }

    public string ReadyToProceedNow { get; set; }

    public decimal TotalArriveAbroadBudget { get; set; }

    public string AreFundsAvailableNow { get; set; }

    public string TryYourLuckWithChosenCountryOrNot { get; set; }

    public string DateApplied { get; set; }
    
    public bool OnboardingComplete { get; set; } = false;
}