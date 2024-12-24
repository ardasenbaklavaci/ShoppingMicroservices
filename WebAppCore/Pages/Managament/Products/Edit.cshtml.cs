using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using WebAppCore.Models;


namespace WebAppCore.Pages.Managament.Products
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public IFormFile? UploadedImage { get; set; }
        public byte[]? CurrentImageData { get; set; } // Holds the current image data
        public Image? image { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("ProductAPI");
            Product = await client.GetFromJsonAsync<Product>($"Product/{id}");

            if (Product == null)
            {
                return NotFound();
            }

            if (Product.ImageID.HasValue)
            {
                var imagesClient = _httpClientFactory.CreateClient("ImageAPI");
                CurrentImageData = await imagesClient.GetByteArrayAsync($"/api/Image/{Product.ImageID.Value}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                //return Page();
            }

            var imagesClient = _httpClientFactory.CreateClient("ImageAPI");
            var productsClient = _httpClientFactory.CreateClient("ProductAPI");

            

            // Handle image upload
            if (UploadedImage != null)
            {
                // Prepare the content
                using var formData = new MultipartFormDataContent();
                using var fileStream = UploadedImage.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(UploadedImage.ContentType);

                formData.Add(fileContent, "file", UploadedImage.FileName);

                // Send the request
                var ImageResponse = await imagesClient.PostAsync("/api/Image", formData);
               

                if (ImageResponse.IsSuccessStatusCode)
                {
                    // Handle successful response (e.g., log, update UI, etc.)
                    var responseContent = await ImageResponse.Content.ReadAsStringAsync();
                    var createdImage = JsonSerializer.Deserialize<Image>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (createdImage != null)
                    {
                        int insertedImageId = createdImage.Id;
                        // Use the ID as needed
                        Console.WriteLine($"Inserted Image ID: {insertedImageId}");

                        Product.ImageID = insertedImageId;                     

                        if (Product.Summary == null)
                        {
                            Product.Summary = "";
                        }
                        if (Product.Description == null)
                        {
                            Product.Description = "";
                        }
                        if (Product.Category == null)
                        {
                            Product.Category = "";
                        }

                        var ProductResponse = await productsClient.PutAsJsonAsync($"/api/Product/{Product.Id}", Product);

                        
                    }


                }
                else
                {
                    // Handle failure response
                    var imageerror = await ImageResponse.Content.ReadAsStringAsync();                                       
                    throw new Exception($"Failed to upload image: {ImageResponse.StatusCode} - {imageerror}");                  
                }
            }
            else
            {
                Product temp = await productsClient.GetFromJsonAsync<Product>($"Product/{Product.Id}");
                Product.ImageID = temp.ImageID;

                if (Product.Summary == null)
                {
                    Product.Summary = "";
                }
                if (Product.Description == null)
                {
                    Product.Description = "";
                }
                if (Product.Category == null)
                {
                    Product.Category = "";
                }

                var ProductResponse = await productsClient.PutAsJsonAsync($"/api/Product/{Product.Id}", Product);
            }

            return Page();
        }
    }

}
