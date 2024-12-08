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
        private readonly IWebHostEnvironment _env;

        public ImageController(IImageRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var image = await _repository.GetByIdAsync(id);
            if (image == null) return NotFound();
            return PhysicalFile(image.FilePath, image.ContentType);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var filePath = Path.Combine(_env.ContentRootPath, "Uploads", file.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var image = new Image
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                FilePath = filePath,
                UploadedAt = DateTime.UtcNow
            };

            var createdImage = await _repository.AddAsync(image);
            return CreatedAtAction(nameof(GetById), new { id = createdImage.Id }, createdImage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var image = await _repository.GetByIdAsync(id);
            if (image == null) return NotFound();

            if (System.IO.File.Exists(image.FilePath))
                System.IO.File.Delete(image.FilePath);

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
