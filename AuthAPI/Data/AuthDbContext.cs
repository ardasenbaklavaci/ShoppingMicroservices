using Microsoft.EntityFrameworkCore;
using AuthAPI.Models;
using System.Collections.Generic;

namespace AuthAPI.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}