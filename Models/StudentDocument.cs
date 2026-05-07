namespace SGEducationNigeriaMobile.Models;

public class StudentDocument
{
    public int  Id { get; set; }
    public string Title { get; set; }
    public string URL { get; set; }
    public string DocumentType { get; set; }
    public string Size { get; set; }
    public int DocumentCategoryId { get; set; }
    public int StudentId { get; set; }
    public DateTime DateUploaded { get; set; }
}