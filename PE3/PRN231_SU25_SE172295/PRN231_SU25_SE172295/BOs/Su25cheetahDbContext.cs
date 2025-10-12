using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace BOs;

public partial class Su25cheetahDbContext : DbContext
{
    public Su25cheetahDbContext()
    {
    }

    public Su25cheetahDbContext(DbContextOptions<Su25cheetahDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CheetahAccount> CheetahAccounts { get; set; }

    public virtual DbSet<CheetahProfile> CheetahProfiles { get; set; }

    public virtual DbSet<CheetahType> CheetahTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()

    {
        IConfiguration configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnectionString"];
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CheetahAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.ToTable("CheetahAccount");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<CheetahProfile>(entity =>
        {
            entity.ToTable("CheetahProfile");

            entity.Property(e => e.CareNeeds).HasMaxLength(1500);
            entity.Property(e => e.Characteristics).HasMaxLength(2000);
            entity.Property(e => e.CheetahName).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CheetahType).WithMany(p => p.CheetahProfiles)
                .HasForeignKey(d => d.CheetahTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CheetahProfile_CheetahType");
        });

        modelBuilder.Entity<CheetahType>(entity =>
        {
            entity.ToTable("CheetahType");

            entity.Property(e => e.CheetahTypeName).HasMaxLength(250);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Origin).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
