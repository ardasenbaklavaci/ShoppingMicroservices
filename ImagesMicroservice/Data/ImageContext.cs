using Microsoft.EntityFrameworkCore;
using ImagesMicroservice.Models;
using System.Collections.Generic;

namespace ImagesMicroservice.Data
{
    public class ImageContext : DbContext
    {
        public ImageContext(DbContextOptions<ImageContext> options) : base(options) { }
        public DbSet<Image> Images { get; set; }
    }
}
