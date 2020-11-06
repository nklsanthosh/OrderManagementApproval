using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OrderManagementApproval.Models
{
    public partial class OrderManagementContext : DbContext
    {
        public OrderManagementContext()
        {
        }

        public OrderManagementContext(DbContextOptions<OrderManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApprovalStatus> ApprovalStatus { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Designation> Designation { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeDepartmentMapping> EmployeeDepartmentMapping { get; set; }
        public virtual DbSet<IndentApproval> IndentApproval { get; set; }
        public virtual DbSet<IndentDetails> IndentDetails { get; set; }
        public virtual DbSet<IndentMaster> IndentMaster { get; set; }
        public virtual DbSet<ItemCategory> ItemCategory { get; set; }
        public virtual DbSet<ItemMaster> ItemMaster { get; set; }
        public virtual DbSet<LocationCode> LocationCode { get; set; }
        public virtual DbSet<RoleMapping> RoleMapping { get; set; }
        public virtual DbSet<RolesMaster> RolesMaster { get; set; }
        public virtual DbSet<UnitMaster> UnitMaster { get; set; }
        public virtual DbSet<UserMaster> UserMaster { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApprovalStatus>(entity =>
            {
                entity.Property(e => e.ApprovalStatusId).HasColumnName("ApprovalStatusID");

                entity.Property(e => e.ApprovalStatus1)
                    .IsRequired()
                    .HasColumnName("ApprovalStatus")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Designation>(entity =>
            {
                entity.Property(e => e.DesignationId).HasColumnName("DesignationID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Designation1)
                    .IsRequired()
                    .HasColumnName("Designation")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DesignationId).HasColumnName("DesignationID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.JoiningDate).HasColumnType("date");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");
            });

            modelBuilder.Entity<EmployeeDepartmentMapping>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.EmployeeDepartmentMappingId)
                    .HasColumnName("EmployeeDepartmentMappingID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<IndentApproval>(entity =>
            {
                entity.Property(e => e.IndentApprovalId).HasColumnName("IndentApprovalID");

                entity.Property(e => e.ApprovalId).HasColumnName("ApprovalID");

                entity.Property(e => e.ApprovalStatusId).HasColumnName("ApprovalStatusID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IndentId).HasColumnName("IndentID");

                entity.Property(e => e.Remarks).HasMaxLength(100);
            });

            modelBuilder.Entity<IndentDetails>(entity =>
            {
                entity.Property(e => e.IndentDetailsId).HasColumnName("IndentDetailsID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IndentId).HasColumnName("IndentID");

                entity.Property(e => e.ItemCategoryId).HasColumnName("ItemCategoryID");

                entity.Property(e => e.ItemMasterId).HasColumnName("ItemMasterID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks).HasMaxLength(100);

                entity.Property(e => e.UnitId).HasColumnName("UnitID");
            });

            modelBuilder.Entity<IndentMaster>(entity =>
            {
                entity.HasKey(e => e.IndentId);

                entity.Property(e => e.IndentId).HasColumnName("IndentID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.LocationCodeId).HasColumnName("LocationCode_Id");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks).HasMaxLength(50);
            });

            modelBuilder.Entity<ItemCategory>(entity =>
            {
                entity.Property(e => e.ItemCategoryId).HasColumnName("ItemCategoryID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.ItemCategoryName).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ItemMaster>(entity =>
            {
                entity.Property(e => e.ItemMasterId).HasColumnName("ItemMasterID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.ItemCategoryId).HasColumnName("ItemCategoryID");

                entity.Property(e => e.ItemCode).HasMaxLength(50);

                entity.Property(e => e.ItemName).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TechnicalSpecification).HasMaxLength(150);

                entity.HasOne(d => d.ItemCategory)
                    .WithMany(p => p.ItemMaster)
                    .HasForeignKey(d => d.ItemCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemMaster_ItemCategory");
            });

            modelBuilder.Entity<LocationCode>(entity =>
            {
                entity.HasKey(e => e.LocationCodeId)
                    .HasName("PK_LocationCode_1")
                    .IsClustered(false);

                entity.HasIndex(e => e.LocationId)
                    .HasName("ClusteredIndex-LocationCode")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.LocationCodeId).HasColumnName("LocationCode_Id");

                entity.Property(e => e.LocationId).HasColumnName("Location_Id");

                entity.Property(e => e.LocationName)
                    .IsRequired()
                    .HasColumnName("Location_Name")
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<RoleMapping>(entity =>
            {
                entity.Property(e => e.RoleMappingId).HasColumnName("RoleMappingID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<RolesMaster>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UnitMaster>(entity =>
            {
                entity.Property(e => e.UnitMasterId).HasColumnName("UnitMasterID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.UserId).ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
