using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SGEducationNigeriaMobile.Models;
using SGEducationNigeriaMobile.Pages.User;

namespace SGEducationNigeriaMobile.Services;

public class ApiService
{
  
    private readonly HttpClient _client;

    public ApiService(HttpClient client)
    {
        _client = client;
    }

    // public async Task<String> RegisterStudent(object student)
    // {
    //     var json = JsonSerializer.Serialize(student);
    //     var content = new StringContent(json, Encoding.UTF8, "application/json");
    //
    //     var response = await _client.PostAsync("http://83.147.39.216/api/students", content);
    //
    //     // return response.IsSuccessStatusCode;
    //
    //     return await response.Content.ReadAsStringAsync();
    // }
    
    
    public async Task<Student> GetStudentByStudentId(int studentId)
    {
        return await _client.GetFromJsonAsync<Student>(
            $"http://api.sgeducationnigerialtd.com/api/students/{studentId}"
        );
    }
    
    public async Task<Student?> RegisterStudent(object student)
    {
        var json = JsonSerializer.Serialize(student);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("http://api.sgeducationnigerialtd.com/api/students", content);

        if (!response.IsSuccessStatusCode)
            return null;

        var responseJson = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<Student>(responseJson,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }
    
    
    
    // ✅ LOGIN (if your API supports it)
    public async Task<Student?> Login(string email, string password)
    {
        var payload = new { email, password };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        
        var response = await _client.PostAsync(
            "http://api.sgeducationnigerialtd.com/api/students/login",
            content
        );
        

        if (!response.IsSuccessStatusCode)
            return null;

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Student>(responseJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    // // ✅ UPDATE (Wizard Step Save)
    // public async Task<bool> UpdateStudent(int id, object updatedData)
    // {
    //     var json = JsonSerializer.Serialize(updatedData);
    //     var content = new StringContent(json, Encoding.UTF8, "application/json");
    //
    //     var response = await _client.PutAsync($"http://api.sgeducationnigerialtd.com/api/students/{id}", content);
    //
    //     return response.IsSuccessStatusCode;
    // }
    
    public async Task<bool> UpdateStudent(int id, object updatedData)
    {
        HttpResponseMessage response = null;

        try
        {
            var json = JsonSerializer.Serialize(updatedData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            });

            Debug.WriteLine(json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            response = await _client.PutAsync(
                $"http://api.sgeducationnigerialtd.com/api/students/{id}",
                content
            );
        }
        catch (Exception ex)
        {
            Debug.WriteLine("XXXXXXXXXXXXX:::::::::: " + ex.Message);
            return false; // important!
        }

        return response != null && response.IsSuccessStatusCode;
    }


    
    public async Task<bool> SendPasswordResetAsync(string email)
    {
        var response = await _client.PostAsJsonAsync($"http://api.sgeducationnigerialtd.com/api/auth/forgot-password", new { Email = email });
        return response.IsSuccessStatusCode;
    }

    
    public async Task<bool> VerifyOtpAsync(string email, string otp)
    {
        var response = await _client.PostAsJsonAsync($"http://api.sgeducationnigerialtd.com/api/auth/verify-otp", new { Email = email, Otp = otp });
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> ResendOtpAsync(string email)
    {
        var response = await _client.PostAsJsonAsync($"http://api.sgeducationnigerialtd.com/api/auth/resend-otp", new { Email = email });
        return response.IsSuccessStatusCode;
    }

    
    public async Task<bool> ResetPasswordAsync(string email, string newPassword)
    {
        var response = await _client.PostAsJsonAsync($"http://api.sgeducationnigerialtd.com/api/auth/reset-password",
            new { Email = email, NewPassword = newPassword });

        return response.IsSuccessStatusCode;
    }


    
    
    
    //***************************************************************************************************
    //STUDENT DOCUMENTS SECTION
    //***************************************************************************************************
    public async Task<IEnumerable<StudentDocument>> GetStudentDocuments(int studentId)
    {
        return await _client.GetFromJsonAsync<IEnumerable<StudentDocument>>(
            $"http://api.sgeducationnigerialtd.com/api/studentdocument/student/{studentId}"
        );
    }
    
    public async Task<IEnumerable<Article>> GetArticlesAsync()
    {
    
        // var response = await _client.GetAsync("/api/article");
        // response.EnsureSuccessStatusCode();
        //
        // var json = await response.Content.ReadAsStringAsync();
        // return JsonSerializer.Deserialize<List<Article>>(json,
        //     new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //
        return await _client.GetFromJsonAsync<IEnumerable<Article>>(
            $"http://api.sgeducationnigerialtd.com/api/article"
        );
     
    }


    // public async Task UploadStudentDocument(int studentId, string fileName, Stream fileStream)
    // {
    //     var content = new MultipartFormDataContent();
    //     content.Add(new StreamContent(fileStream), "file", fileName);
    //
    //     await _client.PostAsync($"students/{studentId}/documents/upload", content);
    // }
    //
    
    
    // public async Task UploadStudentDocument(int studentId, string fileName, Stream fileStream)
    // {
    //     var content = new MultipartFormDataContent();
    //     var streamContent = new StreamContent(fileStream);
    //
    //     streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
    //
    //     // MUST MATCH IFormFile file
    //     content.Add(streamContent, "file", fileName);
    //
    //     var response = await _client.PostAsync($"http://83.147.39.216/api/studentdocument/upload/{studentId}", content);
    //
    //     response.EnsureSuccessStatusCode();
    // }
    
    
    public async Task UploadStudentDocument(int studentId, string fileName, Stream fileStream)
    {
        var content = new MultipartFormDataContent();

        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        // MUST MATCH IFormFile file
        content.Add(fileContent, "file", fileName);

        var response = await _client.PostAsync(
            $"http://api.sgeducationnigerialtd.com/api/studentdocument/upload/{studentId}",
            content
        );


        response.EnsureSuccessStatusCode();
    }

    

    public async Task DeleteStudentDocument(int documentId)
    {
        await _client.DeleteAsync($"http://api.sgeducationnigerialtd.com/api/studentdocument/documents/{documentId}");
    }


    
    
    
    //***************************************************************************************************
    //COUNTRY SECTION
    //***************************************************************************************************
    public async Task<IEnumerable<Country>> GetCountries()
    {
        return await _client.GetFromJsonAsync<IEnumerable<Country>>(
            $"http://api.sgeducationnigerialtd.com/api/countries"
        );
    }
    
    public async Task<Country> GetCountry(int id)
    {
        return await _client.GetFromJsonAsync<Country>(
            $"http://api.sgeducationnigerialtd.com/api/countries/{id}"
        );
    }

    
    public async Task<Country> GetCountryByCountryName(string countryName)
    {
        return await _client.GetFromJsonAsync<Country>(
            $"http://api.sgeducationnigerialtd.com/api/countries/by-name/{countryName}"
        );
    }
    
    
    
    
    
    
    
    //***************************************************************************************************
    //STUDENT COUNTRY OF PREFERENCE SECTION
    //***************************************************************************************************
    public async Task<IEnumerable<StudentCountryOfPreference>> GetStudentCountryOfPreferences()
    {
        return await _client.GetFromJsonAsync<IEnumerable<StudentCountryOfPreference>>(
            $"http://api.sgeducationnigerialtd.com/api/studentCountryOfPreferences"
        );
    }
    
    public async Task<IEnumerable<StudentCountryOfPreference>> GetStudentCountryOfPreferenceByStudentId(int studentId)
    {
        // return await _client.GetFromJsonAsync<IEnumerable<StudentCountryOfPreference>>(
        //     $"http://api.sgeducationnigerialtd.com/api/studentCountryOfPreferences/by-studentid/{studentId}"
        // );
        //
        
        try
        {
            return await _client.GetFromJsonAsync<IEnumerable<StudentCountryOfPreference>> (
                $"http://api.sgeducationnigerialtd.com/api/studentCountryOfPreferences/by-studentid/{studentId}");
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Enumerable.Empty<StudentCountryOfPreference>();

            throw;
        }
    }
    
    
    
    
    // public async Task<StudentCountryOfPreference?> CreateStudentCountryOfPreference(object studentCountryOfPreference)
    // {
    //     var json = JsonSerializer.Serialize(studentCountryOfPreference);
    //     var content = new StringContent(json, Encoding.UTF8, "application/json");
    //
    //     var response = await _client.PostAsync("http://api.sgeducationnigerialtd.com/api/studentCountryOfPreferences", content);
    //
    //     if (!response.IsSuccessStatusCode)
    //         return null;
    //
    //     var responseJson = await response.Content.ReadAsStringAsync();
    //
    //     return JsonSerializer.Deserialize<StudentCountryOfPreference>(responseJson,
    //         new JsonSerializerOptions
    //         {
    //             PropertyNameCaseInsensitive = true
    //         });
    // }
    
    
    
    public async Task<StudentCountryOfPreference?> CreateStudentCountryOfPreference(object studentCountryOfPreference)
    {
        var json = JsonSerializer.Serialize(studentCountryOfPreference);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(
            "http://api.sgeducationnigerialtd.com/api/studentCountryOfPreferences",
            content
        );

        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"API Error {response.StatusCode}: {responseBody}"
            );
        }

        return JsonSerializer.Deserialize<StudentCountryOfPreference>(
            responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
    }


    public async Task DeleteStudentCountryOfPreference(int id)
    {
        var response = await _client.DeleteAsync($"http://api.sgeducationnigerialtd.com/api/StudentCountryOfPreferences/{id}");

        if (!response.IsSuccessStatusCode)
            throw new Exception("Delete failed");
    }

    public async Task DeleteStudentCountryOfPreferenceByCountryIdAndStudentId(int countryId, int studentId)
    {
        var response = await _client.DeleteAsync($"http://api.sgeducationnigerialtd.com/api/StudentCountryOfPreferences/{countryId}/{studentId}");

        if (!response.IsSuccessStatusCode)
            throw new Exception("Delete failed");
    }
} 