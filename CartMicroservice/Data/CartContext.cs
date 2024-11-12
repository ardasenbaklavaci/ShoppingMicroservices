using CartMicroservice.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CartMicroservice.Data
{
    public class CartContext : DbContext
    {
        public CartContext(DbContextOptions<CartContext> options) : base(options) { }
        public DbSet<CartItem> CartItems { get; set; }
    }

}
