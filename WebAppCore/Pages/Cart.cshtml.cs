using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class CartModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    public decimal CartTotal => CartItems.Sum(item => item.Price * item.Quantity);
    public bool IsLoggedIn { get; private set; }
    public string ErrorMessage { get; set; }
    public CartModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        // Check if user is logged in
        var token = HttpContext.Session.GetString("JWTToken");
        var userId = HttpContext.Session.GetString("User");

        if(userId != null)
        {
            IsLoggedIn = true;
        }

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Index"); // Redirect if not logged in
        }

        try
        {
            var CartClient = _clientFactory.CreateClient();
            CartClient.BaseAddress = new Uri("https://localhost:7246/"); // CartMicroservice
            CartClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Fetch cart items for the logged-in user
            var CartResponse = await CartClient.GetAsync($"api/cart/items?userId={userId}"); //cart items get userId


            if (CartResponse.IsSuccessStatusCode)
            {
                CartItems = await CartResponse.Content.ReadFromJsonAsync<List<CartItem>>();

                
            }
            else
            {
                ErrorMessage = "Failed to load cart items.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred: {ex.Message}";
        }

        return Page();
    }
}

public class CartItem
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
