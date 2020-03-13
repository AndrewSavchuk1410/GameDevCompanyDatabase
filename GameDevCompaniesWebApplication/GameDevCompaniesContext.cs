using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GameDevCompaniesWebApplication
{
    public partial class GameDevCompaniesContext : DbContext
    {
        public GameDevCompaniesContext()
        {
        }

        public GameDevCompaniesContext(DbContextOptions<GameDevCompaniesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ComputerGames> ComputerGames { get; set; }
        public virtual DbSet<Distributors> Distributors { get; set; }
        public virtual DbSet<GameDevCompanies> GameDevCompanies { get; set; }
        public virtual DbSet<GamesDevelopers> GamesDevelopers { get; set; }
        public virtual DbSet<GamesDistributors> GamesDistributors { get; set; }
        public virtual DbSet<GamesGenres> GamesGenres { get; set; }
        public virtual DbSet<GamesPlatforms> GamesPlatforms { get; set; }
        public virtual DbSet<GamesPublishers> GamesPublishers { get; set; }
        public virtual DbSet<Genres> Genres { get; set; }
        public virtual DbSet<Platforms> Platforms { get; set; }
        public virtual DbSet<Subsidiaries> Subsidiaries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-2SAJVJC\\SQLEXPRESS;Database=GameDevCompanies;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComputerGames>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Budget).HasColumnType("money");

                entity.Property(e => e.Engine)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Distributors>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasMaxLength(1000);
            });

            modelBuilder.Entity<GameDevCompanies>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DirectorFullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GamesDevelopers>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID");
                    //.ValueGeneratedNever();

                entity.Property(e => e.SubsidiariesId).HasColumnName("SubsidiariesID");
                
                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.HasOne(d => d.Subsidiaries)
                    .WithMany(p => p.GamesDevelopers)
                    .HasForeignKey(d => d.SubsidiariesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameDeveloper_Subsidiaries");
                
                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GamesDevelopers)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameDeveloper_ComputerGame");

                
            });

            modelBuilder.Entity<GamesDistributors>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DistributorId).HasColumnName("DistributorID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.HasOne(d => d.Distributor)
                    .WithMany(p => p.GamesDistributors)
                    .HasForeignKey(d => d.DistributorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameDistributor_Distributor");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GamesDistributors)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameDistributor_ComputerGame");
            });

            modelBuilder.Entity<GamesGenres>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID");
                    //.ValueGeneratedNever();

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.GenreId).HasColumnName("GenreID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GamesGenres)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameGenre_ComputerGame");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.GamesGenres)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameGenre_Genre");
            });

            modelBuilder.Entity<GamesPlatforms>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID");
                    //.ValueGeneratedNever();

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.PlatformId).HasColumnName("PlatformID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GamesPlatforms)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GamePlatform_ComputerGame");

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.GamesPlatforms)
                    .HasForeignKey(d => d.PlatformId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GamePlatform_Platform");
            });

            modelBuilder.Entity<GamesPublishers>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.GamesPublishers)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GamePublisher_GameDevCompany");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GamesPublishers)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GamePublisher_ComputerGame");
            });

            modelBuilder.Entity<Genres>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasMaxLength(1000);
            });

            modelBuilder.Entity<Platforms>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasMaxLength(1000);
            });

            modelBuilder.Entity<Subsidiaries>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ManagerFullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Subsidiaries)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subsidiaries_GameDevCompany");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
