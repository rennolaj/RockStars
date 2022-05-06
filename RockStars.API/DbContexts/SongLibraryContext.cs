using RockStars.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace RockStars.API.DbContexts
{
    public class SongLibraryContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Song> Songs { get; set; }

        public SongLibraryContext(DbContextOptions<SongLibraryContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>()
                .Property(a => a.Id)
                .UseIdentityColumn();
                
            modelBuilder.Entity<Song>()
                .Property(a => a.Id)
                .UseIdentityColumn();

            base.OnModelCreating(modelBuilder);
        }
    }
}
