using ImagesMicroservice.Data;
using ImagesMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageMicroservices.Repositories
{
    public interface IImageRepository
    {
        Task<IEnumerable<Image>> GetAllAsync();
        Task<Image?> GetByIdAsync(int id);
        Task<Image> AddAsync(Image image);
        Task DeleteAsync(int id);
    }

    public class ImageRepository : IImageRepository
    {
        private readonly ImageContext _context;

        public ImageRepository(ImageContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Image>> GetAllAsync() => await _context.Images.ToListAsync();

        public async Task<Image?> GetByIdAsync(int id) => await _context.Images.FindAsync(id);

        public async Task<Image> AddAsync(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task DeleteAsync(int id)
        {
            var image = await GetByIdAsync(id);
            if (image != null)
            {
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();
            }
        }
    }
}
