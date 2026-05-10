using Microsoft.EntityFrameworkCore;
using SharedLibrary;

namespace Server.Data;

public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options) {
    public DbSet<User> Users { get; set; }
    
}