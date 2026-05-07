using Microsoft.Maui.Storage;
using SGEducationNigeriaMobile.Models;
using SGEducationNigeriaMobile.Services;

namespace SGEducationNigeriaMobile.Pages.User;

public partial class DocumentUploadPage : ContentPage
{
    private readonly ApiService _api;

    public List<StudentDocument> UploadedDocuments { get; set; } = new();

    public DocumentUploadPage(ApiService api)
    {
        InitializeComponent();
        _api = api; 
       LoadDocuments();
    }

    private async void LoadDocuments()
    {
        var docs = await _api.GetStudentDocuments(SessionManager.StudentId);
        UploadedDocuments = docs.ToList();
        DocumentsCollectionView.ItemsSource = UploadedDocuments;
    }

    private async void OnSelectDocumentClicked(object sender, EventArgs e)
    {
        try
        {
            // var result = await FilePicker.PickAsync(new PickOptions
            // {
            //     PickerTitle = "Select a document"
            // });
            
            
            var customTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { "public.data", "public.content", "public.item", "public.pdf", "com.microsoft.word.doc", "org.openxmlformats.wordprocessingml.document" } }
            });

            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select a document",
                FileTypes = customTypes
            });


            if (result != null)
            {
                
                using var stream = await result.OpenReadAsync();

                await _api.UploadStudentDocument(
                    SessionManager.StudentId,
                    result.FileName,
                    stream
                );

                await DisplayAlert("Success", "Document uploaded successfully", "OK");
                LoadDocuments();
                
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnDeleteDocumentClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is StudentDocument doc)
        {
            bool confirm = await DisplayAlert("Delete", $"Delete {doc.Title}?", "Yes", "No");

            if (confirm)
            {
                await _api.DeleteStudentDocument(doc.Id);
                LoadDocuments();
            }
        }
    }

    private async void OnViewDocumentClicked(object sender, EventArgs e)
    {
        try
        {

            if (sender is Button btn && btn.BindingContext is StudentDocument doc)
            {
                await Launcher.OpenAsync(doc.URL);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
