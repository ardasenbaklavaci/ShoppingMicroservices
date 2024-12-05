using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebAppCore.Models;

public class CartModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    public List<Product> products { get; set; } = new List<Product>();
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

            var ProductClient = _clientFactory.CreateClient();
            ProductClient.BaseAddress = new Uri("https://localhost:7259/"); // ProductMicroservice
            ProductClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Fetch products
            var ProductResponse = await ProductClient.GetAsync($"api/product"); //get products...

            if (ProductResponse.IsSuccessStatusCode)
            {
                products = await ProductResponse.Content.ReadFromJsonAsync<List<Product>>();

                if (CartResponse.IsSuccessStatusCode)
                {
                    CartItems = await CartResponse.Content.ReadFromJsonAsync<List<CartItem>>();                  

                    if(CartItems!=null) { 
                        foreach (var item in CartItems)
                        {
                            item.ProductName = products.ToList().Where(a => a.Id == int.Parse(item.ProductId)).FirstOrDefault().Name;   
                            item.Price = products.ToList().Where(a => a.Id == int.Parse(item.ProductId)).FirstOrDefault().Price;
                            
                        }
                    }
                }
                else
                {
                    ErrorMessage = "Failed to load cart items.";
                }
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
