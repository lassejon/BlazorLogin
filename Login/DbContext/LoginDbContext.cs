using Microsoft.EntityFrameworkCore;
using Login.Models;

namespace Login.DbContext;

public partial class LoginDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public LoginDbContext()
    {
    }

    public LoginDbContext(DbContextOptions<LoginDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnOrder(0);
            entity.Property(e => e.Created)
                .HasColumnOrder(4)
                .HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnOrder(2);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnOrder(1);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(100)
                .HasColumnOrder(3);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
