using ImagesMicroservice.Models;
using ImageMicroservices.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ImagesMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _repository;

        public ImageController(IImageRepository repository)
        {
            _repository = repository;
        }

        // Get all images metadata
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

        // Get a single image by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var image = await _repository.GetByIdAsync(id);
            if (image == null) return NotFound();

            // Return the binary data as an image
            return File(image.Data, image.ContentType);
        }

        // Upload an image
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var image = new Image
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Data = memoryStream.ToArray(),
                UploadedAt = DateTime.UtcNow
            };

            var createdImage = await _repository.AddAsync(image);
            return CreatedAtAction(nameof(GetById), new { id = createdImage.Id }, createdImage);
        }

        // Delete an image by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var image = await _repository.GetByIdAsync(id);
            if (image == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
