using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Q1.Models;

namespace Q1.Data;

public partial class PE_PRN_Fall22B1Context : DbContext
{
    public PE_PRN_Fall22B1Context()
    {
    }

    public PE_PRN_Fall22B1Context(DbContextOptions<PE_PRN_Fall22B1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Producer> Producers { get; set; }

    public virtual DbSet<Star> Stars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Connection string should come from appsettings.json via Program.cs registration.
            // If you hit this branch, DbContext was not configured correctly in DI.
            throw new InvalidOperationException(
                "DbContext is not configured. Please configure the connection string 'ConnectionStrings:MyCnn' in Program.cs.");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(e => e.Title).IsFixedLength();
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasOne(d => d.Director).WithMany(p => p.Movies).HasConstraintName("FK_Movies_Directors");

            entity.HasOne(d => d.Producer).WithMany(p => p.Movies).HasConstraintName("FK_Movies_Producers");

            entity.HasMany(d => d.Genres).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Movie_Genre_Genres"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Movie_Genre_Movies"),
                    j =>
                    {
                        j.HasKey("MovieId", "GenreId");
                        j.ToTable("Movie_Genre");
                    });

            entity.HasMany(d => d.Stars).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieStar",
                    r => r.HasOne<Star>().WithMany()
                        .HasForeignKey("StarId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Movie_Star_Stars"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Movie_Star_Movies"),
                    j =>
                    {
                        j.HasKey("MovieId", "StarId");
                        j.ToTable("Movie_Star");
                    });
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Productions");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
