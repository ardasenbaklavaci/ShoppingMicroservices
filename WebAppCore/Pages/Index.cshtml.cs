using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebAppCore.Models;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public List<Product> Products { get; set; } = new();
    public List<Image> Images { get; set; }
    public async Task OnGetAsync()
    {
        var productClient = _httpClientFactory.CreateClient("ProductAPI");
        Products = await productClient.GetFromJsonAsync<List<Product>>("/api/Product");

        var imageClient = _httpClientFactory.CreateClient("ImageAPI");
        Images = await imageClient.GetFromJsonAsync<List<Image>>("/api/Image");


        if (Images != null)
        {
            foreach (var image in Images)
            {
                Console.WriteLine($"Image ID: {image.Id}, File Name: {image.FileName}");
            }
        }

        foreach(var _product in Products)
        {
            if (_product.ImageID.HasValue)
            {

            }
        }


    }
}
