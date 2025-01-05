using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace LampManufacturingAPI.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BillOfMaterial> BillOfMaterials { get; set; }

    public virtual DbSet<Bin> Bins { get; set; }

    public virtual DbSet<Configuration> Configurations { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<MaterialTransaction> MaterialTransactions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<Tray> Trays { get; set; }

    public virtual DbSet<WorkstationJob> WorkstationJobs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MAHAPARK;Database=Knot;Integrated Security=True;TrustServerCertificate=True;");


    // https://learn.microsoft.com/en-us/ef/core/modeling/keyless-entity-types?tabs=data-annotations
    public DbSet<BinsWithWorkstationjob> BinsWithWorkstationjobs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BillOfMaterial>(entity =>
        {
            entity.HasKey(e => e.Bomid);

            entity.Property(e => e.Bomid).HasColumnName("BOMID");
            entity.Property(e => e.ChildProductId).HasColumnName("ChildProductID");
            entity.Property(e => e.ParentProductId).HasColumnName("ParentProductID");

            entity.HasOne(d => d.ChildProduct).WithMany(p => p.BillOfMaterialChildProducts)
                .HasForeignKey(d => d.ChildProductId)
                .HasConstraintName("FK_BillOfMaterials_ChildProductID");

            entity.HasOne(d => d.ParentProduct).WithMany(p => p.BillOfMaterialParentProducts)
                .HasForeignKey(d => d.ParentProductId)
                .HasConstraintName("FK_BillOfMaterials_ParentProductID");
        });

        modelBuilder.Entity<Bin>(entity =>
        {
            entity.Property(e => e.BinId).HasColumnName("BinID");
            entity.Property(e => e.LastRepTime).HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.StationId).HasColumnName("StationID");

            entity.HasOne(d => d.Product).WithMany(p => p.Bins)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Bins_Product");

            entity.HasOne(d => d.Station).WithMany(p => p.Bins)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK_Bins_Station");

            // https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/breaking-changes?tabs=v7#sqlserver-tables-with-triggers
            entity.ToTable(tb => tb.HasTrigger("trigger_Bin_IsReplenishmentNeeded"));
        });

        modelBuilder.Entity<Configuration>(entity =>
        {
            entity.ToTable("Configuration");

            entity.Property(e => e.ConfigurationId)
                .HasMaxLength(50)
                .HasColumnName("ConfigurationID");
            entity.Property(e => e.BatchNumber).HasMaxLength(50);
            entity.Property(e => e.EmployeeName).HasMaxLength(50);
            entity.Property(e => e.OrderAmount).HasColumnType("money");
            entity.Property(e => e.OrderNumber).HasMaxLength(50);
            entity.Property(e => e.PartName1).HasMaxLength(50);
            entity.Property(e => e.PartName2).HasMaxLength(50);
            entity.Property(e => e.PartName3).HasMaxLength(50);
            entity.Property(e => e.PartName4).HasMaxLength(50);
            entity.Property(e => e.PartName5).HasMaxLength(50);
            entity.Property(e => e.PartName6).HasMaxLength(50);
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.StationName).HasMaxLength(50);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Inventory");

            entity.Property(e => e.BatchNumber).HasMaxLength(50);
            entity.Property(e => e.ProductId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Product");
        });

        modelBuilder.Entity<MaterialTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId);

            entity.ToTable("MaterialTransaction");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.BatchNumber).HasMaxLength(50);
            entity.Property(e => e.MovementType).HasMaxLength(50);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.MaterialTransactions)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_MaterialTransaction_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.UnitMeasurementCode).HasMaxLength(50);
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.Property(e => e.StationId).HasColumnName("StationID");
            entity.Property(e => e.StationName).HasMaxLength(50);
        });

        modelBuilder.Entity<Tray>(entity =>
        {
            entity.Property(e => e.TrayId).HasColumnName("TrayID");
            entity.Property(e => e.BatchNumber).HasMaxLength(50);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.StationId).HasColumnName("StationID");

            entity.HasOne(d => d.Product).WithMany(p => p.Trays)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Trays_Product");

            entity.HasOne(d => d.Station).WithMany(p => p.Trays)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK_Trays_Station");

            // https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/breaking-changes?tabs=v7#sqlserver-tables-with-triggers
            entity.ToTable(tb => tb.HasTrigger("trigger_WorkstationJob_YieldQuantity"));
        });

        modelBuilder.Entity<WorkstationJob>(entity =>
        {
            entity.HasKey(e => e.JobId);

            entity.ToTable("WorkstationJob");

            entity.Property(e => e.JobId).HasColumnName("JobID");
            entity.Property(e => e.BatchNumber).HasMaxLength(50);
            entity.Property(e => e.ConfigurationId)
                .HasMaxLength(50)
                .HasColumnName("ConfigurationID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.JobEndDatetime).HasColumnType("datetime");
            entity.Property(e => e.JobStartDatetime).HasColumnType("datetime");
            entity.Property(e => e.OrderAmount).HasColumnType("money");
            entity.Property(e => e.OrderNumber).HasMaxLength(50);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.StationId).HasColumnName("StationID");
            entity.Property(e => e.StationName).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TrayId).HasColumnName("TrayID");

            entity.HasOne(d => d.Employee).WithMany(p => p.WorkstationJobs)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_WorkstationJob_Employee");

            entity.HasOne(d => d.Product).WithMany(p => p.WorkstationJobs)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_WorkstationJob_Product");

            entity.HasOne(d => d.Station).WithMany(p => p.WorkstationJobs)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK_WorkstationJob_Station");

            entity.HasOne(d => d.Tray).WithMany(p => p.WorkstationJobs)
                .HasForeignKey(d => d.TrayId)
                .HasConstraintName("FK_WorkstationJob_Tray");
        });

        // https://learn.microsoft.com/en-us/archive/msdn-magazine/2018/july/data-points-ef-core-2-1-query-types
        modelBuilder
        .Entity<BinsWithWorkstationjob>(
            eb =>
            {
                eb.HasNoKey();
                eb.ToView("view_BinsWithWorkstationjob");
                eb.Property(v => v.BinId).HasColumnName("BinId");
            });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    
    

}
