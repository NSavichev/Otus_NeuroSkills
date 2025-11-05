using Microsoft.EntityFrameworkCore;
using SchulteTable.Core.Enums;

namespace SchulteTable.Data.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<GameResultEntity> GameResults { get; set; }
    public DbSet<UserProfileEntity> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // настройка юзера
        modelBuilder.Entity<UserProfileEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.PreferredGridSize).HasDefaultValue(5);
            entity.Property(e => e.PreferredTheme).HasConversion<string>().HasDefaultValue(ThemeMode.Auto);
        });

        // настройка результата
        modelBuilder.Entity<GameResultEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GridSize).IsRequired();
            entity.Property(e => e.TimeElapsedTicks).IsRequired();
            entity.Property(e => e.DateCompleted).IsRequired();
            entity.Property(e => e.ErrorCount).HasDefaultValue(0);

            // связь с юзером
            entity.HasOne(e => e.UserProfile)
                  .WithMany(p => p.GameResults)
                  .HasForeignKey(e => e.UserProfileId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // индексы на всякий случай
        modelBuilder.Entity<GameResultEntity>()
            .HasIndex(e => new { e.UserProfileId, e.GridSize, e.DateCompleted });
    }
}
