using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppCore.Models;

namespace WebAppCore.Pages.Managament.Products
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public List<Product> Products { get; set; } 
        public List<Image> Images { get; set; } 
        public async void OnGetAsync()
        {
            var productsClient = _httpClientFactory.CreateClient("ProductAPI");
            Products = await productsClient.GetFromJsonAsync<List<Product>>("Product");

            var imagesClient = _httpClientFactory.CreateClient("ImageAPI");

        }
    }
}
