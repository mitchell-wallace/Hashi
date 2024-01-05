using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HashiAPI_1.Models
{
    public partial class hashi_dbContext : DbContext
    {
        public hashi_dbContext()
        {
        }

        public hashi_dbContext(DbContextOptions<hashi_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (System.Environment.GetEnvironmentVariable(
                    "SQLServerDBConnectionString") is not null) {
                        optionsBuilder.UseSqlServer(System.Environment.GetEnvironmentVariable(
                    "SQLServerDBConnectionString")!);
                }
                else { // TODO proper error handling for if program cannot connect to database
                    Console.WriteLine("ERROR: Environment variables required for mappings " +
					    "database connection string is NULL; connection to mappings database is unavailable");
                }
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.PID)
                    .HasName("PK__projects__4A0B0D685F01DFD1");

                entity.ToTable("projects");

                entity.Property(e => e.PID)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("pmap_id");

                entity.Property(e => e.ProjectName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("project_name");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.JiraId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("jira_id");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");

                entity.Property(e => e.WrikeId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("wrike_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UID)
                    .HasName("PK__users__AB6E61650C1582E8");

                entity.ToTable("users");

                entity.Property(e => e.UID)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("umap_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("display_name");

                entity.Property(e => e.JiraId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("jira_id");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");

                entity.Property(e => e.WrikeId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("wrike_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
