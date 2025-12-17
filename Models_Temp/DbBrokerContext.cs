using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Broker.Models_Temp;

public partial class DbBrokerContext : DbContext
{
    public DbBrokerContext()
    {
    }

    public DbBrokerContext(DbContextOptions<DbBrokerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<EmailQueue> EmailQueues { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<ForgetPassword> ForgetPasswords { get; set; }

    public virtual DbSet<Lead> Leads { get; set; }

    public virtual DbSet<LeadFollowup> LeadFollowups { get; set; }

    public virtual DbSet<LeadSource> LeadSources { get; set; }

    public virtual DbSet<LeadTimeline> LeadTimelines { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<LocationPropertyMap> LocationPropertyMaps { get; set; }

    public virtual DbSet<LovMaster> LovMasters { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<PropertyAmenity> PropertyAmenities { get; set; }

    public virtual DbSet<PropertyAmenityMap> PropertyAmenityMaps { get; set; }

    public virtual DbSet<PropertyCategory> PropertyCategories { get; set; }

    public virtual DbSet<PropertyImage> PropertyImages { get; set; }

    public virtual DbSet<PropertyType> PropertyTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleMenuAccess> RoleMenuAccesses { get; set; }

    public virtual DbSet<ServicesMaster> ServicesMasters { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserMenuAccess> UserMenuAccesses { get; set; }

    public virtual DbSet<UserRoleMapping> UserRoleMappings { get; set; }

    public virtual DbSet<UserVendorMapping> UserVendorMappings { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=KL-ITS-DEVELOP2\\SQLEXPRESS;Initial Catalog=dbBroker;Integrated Security=True;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__Areas__70B82048FFACB275");

            entity.Property(e => e.AreaName).HasMaxLength(50);

            entity.HasOne(d => d.City).WithMany(p => p.Areas)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Areas_Cities");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Cities__F2D21B76DBFC2BA6");

            entity.Property(e => e.CityName).HasMaxLength(100);
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasDefaultValue("Gujarat")
                .HasColumnName("state");
        });

        modelBuilder.Entity<EmailQueue>(entity =>
        {
            entity.HasKey(e => e.EmailId).HasName("PK__EmailQue__7ED91ACF19D35AFD");

            entity.ToTable("EmailQueue");

            entity.Property(e => e.Body).IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Subject)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ToEmail)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.UserId, e.RoleId });

            entity.ToTable("Employee");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.ErrorId).HasName("PK__ErrorLog__35856A2A9245DA6E");

            entity.ToTable("ErrorLog");

            entity.Property(e => e.ApplicationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClientIp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ClientIP");
            entity.Property(e => e.ControllerName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ErrorMessage).IsUnicode(false);
            entity.Property(e => e.ErrorType)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RequestPayload).IsUnicode(false);
            entity.Property(e => e.RequestUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.StackTrace).IsUnicode(false);
            entity.Property(e => e.UserAgent)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ForgetPassword>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ForgetPa__3214EC07155ECC49");

            entity.ToTable("ForgetPassword");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Otp)
                .HasMaxLength(10)
                .HasColumnName("OTP");
        });

        modelBuilder.Entity<Lead>(entity =>
        {
            entity.HasKey(e => e.LeadId).HasName("PK__Leads__73EF78FAF5903568");

            entity.Property(e => e.BudgetMax).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.BudgetMin).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Landmark).IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PreferredAreaId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Requirement)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("New");

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.Leads)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK_Leads_User");

            entity.HasOne(d => d.LeadSource).WithMany(p => p.Leads)
                .HasForeignKey(d => d.LeadSourceId)
                .HasConstraintName("FK_Leads_LeadSource");

            entity.HasOne(d => d.PreferredCity).WithMany(p => p.Leads)
                .HasForeignKey(d => d.PreferredCityId)
                .HasConstraintName("FK_Leads_PreferredCity");

            entity.HasOne(d => d.PropertyTypeNavigation).WithMany(p => p.Leads)
                .HasForeignKey(d => d.PropertyType)
                .HasConstraintName("FK_Leads_PropertyTypes");
        });

        modelBuilder.Entity<LeadFollowup>(entity =>
        {
            entity.HasKey(e => e.FollowupId).HasName("PK__LeadFoll__C6356211ADB9C0A7");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Remark).IsUnicode(false);

            entity.HasOne(d => d.Lead).WithMany(p => p.LeadFollowups)
                .HasForeignKey(d => d.LeadId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Followup_Lead");
        });

        modelBuilder.Entity<LeadSource>(entity =>
        {
            entity.HasKey(e => e.LeadSourceId).HasName("PK__LeadSour__9FB37DB3BB167E3C");

            entity.ToTable("LeadSource");

            entity.Property(e => e.LeadSourceId).HasColumnName("LeadSourceID");
            entity.Property(e => e.LeadSourceName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LeadTimeline>(entity =>
        {
            entity.HasKey(e => e.TimelineId).HasName("PK__LeadTime__1DE4F085D526EF64");

            entity.ToTable("LeadTimeline");

            entity.Property(e => e.Action)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.NewValue)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.OldValue)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Lead).WithMany(p => p.LeadTimelines)
                .HasForeignKey(d => d.LeadId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Timeline_Lead");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Location__E7FEA4974BB4678C");

            entity.Property(e => e.LocationName).HasMaxLength(150);
            entity.Property(e => e.Pincode).HasMaxLength(10);

            entity.HasOne(d => d.Area).WithMany(p => p.Locations)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Locations__AreaI__17C286CF");
        });

        modelBuilder.Entity<LocationPropertyMap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC0779E0E50A");

            entity.ToTable("LocationPropertyMap");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Area).WithMany(p => p.LocationPropertyMaps)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK_LPM_Area");

            entity.HasOne(d => d.Property).WithMany(p => p.LocationPropertyMaps)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_LPM_Property");
        });

        modelBuilder.Entity<LovMaster>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LOV_MASTER");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.LovCode).HasColumnName("LOV_Code");
            entity.Property(e => e.LovColumn).HasColumnName("LOV_Column");
            entity.Property(e => e.LovDesc).HasColumnName("LOV_Desc");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.ParentId });

            entity.ToTable("Menu");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E12588DC926");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Message).IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Notifications_Users");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.PropertyId).HasName("PK__Properti__70C9A7351A597973");

            entity.Property(e => e.AreaSqft).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BuilderName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Landmark).IsUnicode(false);
            entity.Property(e => e.OwnerMobile)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.OwnerName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Area).WithMany(p => p.Properties)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK_Properties_Area");

            entity.HasOne(d => d.Category).WithMany(p => p.Properties)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Properties_Category");

            entity.HasOne(d => d.City).WithMany(p => p.Properties)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_Properties_City");

            entity.HasOne(d => d.Type).WithMany(p => p.Properties)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_Properties_Type");
        });

        modelBuilder.Entity<PropertyAmenity>(entity =>
        {
            entity.HasKey(e => e.AmenityId).HasName("PK__Property__842AF50B127D012E");

            entity.Property(e => e.AmenityName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<PropertyAmenityMap>(entity =>
        {
            entity.HasKey(e => new { e.PropertyId, e.AmenityId }).HasName("PK__Property__D88B08655E417404");

            entity.ToTable("PropertyAmenityMap");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Amenity).WithMany(p => p.PropertyAmenityMaps)
                .HasForeignKey(d => d.AmenityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Map_Amenity");

            entity.HasOne(d => d.Property).WithMany(p => p.PropertyAmenityMaps)
                .HasForeignKey(d => d.PropertyId)
                .HasConstraintName("FK_Map_Property");
        });

        modelBuilder.Entity<PropertyCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Property__19093A0B860F425A");

            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<PropertyImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Property__7516F70C9533DEE9");

            entity.Property(e => e.ImageUrl)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.IsPrimary).HasDefaultValue(false);

            entity.HasOne(d => d.Property).WithMany(p => p.PropertyImages)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PropertyImages");
        });

        modelBuilder.Entity<PropertyType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Property__516F03B5C7D2EB62");

            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ProjectDetailTypeAccess).HasMaxLength(500);
        });

        modelBuilder.Entity<RoleMenuAccess>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("RoleMenuAccess");
        });

        modelBuilder.Entity<ServicesMaster>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB00AA6B29667");

            entity.ToTable("ServicesMaster");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.FullDescription).IsUnicode(false);
            entity.Property(e => e.ImageName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ServiceTitle)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ShortDescription)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedBy).HasDefaultValue(0L);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.LastModifiedBy).HasDefaultValue(0L);
            entity.Property(e => e.MobileNumber).HasMaxLength(50);
            entity.Property(e => e.NextChangePasswordDate).HasColumnName("Next_Change_Password_Date");
            entity.Property(e => e.NoOfWrongPasswordAttempts).HasColumnName("No_Of_Wrong_Password_Attempts");
            entity.Property(e => e.Password).HasMaxLength(500);
            entity.Property(e => e.UserName).HasMaxLength(500);
        });

        modelBuilder.Entity<UserMenuAccess>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UserMenuAccess");
        });

        modelBuilder.Entity<UserRoleMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserRoleMapping_1");

            entity.ToTable("UserRoleMapping");
        });

        modelBuilder.Entity<UserVendorMapping>(entity =>
        {
            entity.ToTable("UserVendorMapping");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.UserId, e.RoleId }).HasName("PK_Vendor_1");

            entity.ToTable("Vendor");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ContactNoAlternate).HasColumnName("ContactNo_Alternate");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
