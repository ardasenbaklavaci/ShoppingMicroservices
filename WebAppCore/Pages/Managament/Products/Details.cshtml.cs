using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppCore.Models;

namespace WebAppCore.Pages.Managament.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public Image ProductImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var productClient = _httpClientFactory.CreateClient("ProductAPI");
            Product = await productClient.GetFromJsonAsync<Product>($"/api/Product/{id}");
            if (Product == null) return NotFound();

            if (Product.ImageID.HasValue)
            {
                var imageClient = _httpClientFactory.CreateClient("ImageAPI");
                ProductImage = await imageClient.GetFromJsonAsync<Image>($"/api/Image/{Product.ImageID.Value}");
            }

            return Page();
        }
    }
}
