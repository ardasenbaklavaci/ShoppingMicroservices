using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class LoginModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    [BindProperty]
    public LoginRequest LoginRequest { get; set; }

    public string ErrorMessage { get; set; }

    public LoginModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri("https://localhost:7176/");   // Adjust to your AuthAPI base URL

        HttpResponseMessage response = await client.PostAsJsonAsync("api/Auth/Login", LoginRequest);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            HttpContext.Session.SetString("JWTToken", result.Token);

            // Redirect to the homepage or any secure area
            return RedirectToPage("/Index");
        }
        else
        {
            // Handle invalid login
            ErrorMessage = "Invalid username or password";
            return Page();
        }
    }
}

public class LoginRequest
{
    [JsonPropertyName("email")]
    public string Username { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
}
