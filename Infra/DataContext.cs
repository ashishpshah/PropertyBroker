using Broker.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Data;
using System.Text;

namespace Broker.Infra
{
	public partial class DataContext : DbContext
	{
		public DataContext() { }

		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		public virtual DbSet<AreasMaster> Areas { get; set; }

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

		public virtual DbSet<Properties> Properties { get; set; }

		public virtual DbSet<PropertyAmenity> PropertyAmenities { get; set; }

		public virtual DbSet<PropertyAmenityMap> PropertyAmenityMaps { get; set; }

		public virtual DbSet<PropertyCategory> PropertyCategories { get; set; }

		public virtual DbSet<PropertyType> PropertyTypes { get; set; }

		public virtual DbSet<PropertyImage> PropertyImages { get; set; }

		public virtual DbSet<Role> Roles { get; set; }

		public virtual DbSet<RoleMenuAccess> RoleMenuAccesses { get; set; }

		public virtual DbSet<ServicesMaster> ServicesMasters { get; set; }

		public virtual DbSet<User> Users { get; set; }

		public virtual DbSet<UserMenuAccess> UserMenuAccesses { get; set; }

		public virtual DbSet<UserRoleMapping> UserRoleMappings { get; set; }

		public virtual DbSet<UserVendorMapping> UserVendorMappings { get; set; }

		public virtual DbSet<Vendor> Vendors { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AreasMaster>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Areas__70B8204855D6F241");

				entity.ToTable("Areas", "dbo");

				entity.Property(e => e.Name).HasMaxLength(50);

				//entity.HasOne(d => d.City).WithMany(p => p.Areas)
				//	.HasForeignKey(d => d.CityId)
				//	.OnDelete(DeleteBehavior.ClientSetNull)
				//	.HasConstraintName("FK_Areas_Cities");
			});

			modelBuilder.Entity<City>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Cities__F2D21B7658D7BCB1");

				entity.ToTable("Cities", "dbo");

				entity.Property(e => e.Name).HasMaxLength(100);
				entity.Property(e => e.State)
					.HasMaxLength(100)
					.HasDefaultValue("Gujarat");
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
				entity.HasKey(e => e.Id).HasName("PK__Leads__73EF78FAF5903568");

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
				entity.Property(e => e.Preferred_Area_Id)
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

				//entity.HasOne(d => d.LeadSource).WithMany(p => p.Leads)
				//	.HasForeignKey(d => d)
				//	.HasConstraintName("FK_Leads_LeadSource");

			});

			modelBuilder.Entity<LeadFollowup>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__LeadFoll__C6356211ADB9C0A7");

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

				//entity.HasOne(d => d.Area).WithMany(p => p.Locations)
				//	.HasForeignKey(d => d.AreaId)
				//	.OnDelete(DeleteBehavior.ClientSetNull)
				//	.HasConstraintName("FK__Locations__AreaI__17C286CF");
			});

			modelBuilder.Entity<LocationPropertyMap>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Location__3214EC0779E0E50A");

				entity.ToTable("LocationPropertyMap");

				entity.Property(e => e.CreatedDate).HasColumnType("datetime");
				entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

				//entity.HasOne(d => d.Area).WithMany(p => p.LocationPropertyMaps)
				//	.HasForeignKey(d => d.AreaId)
				//	.HasConstraintName("FK_LPM_Area");

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

			modelBuilder.Entity<Properties>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Properti__70C9A7351A597973");

				entity.ToTable("Properties", "dbo");

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
				entity.Property(e => e.AvailabilityStatus).HasColumnName("Status")
					.HasMaxLength(50)
					.IsUnicode(false)
					.HasDefaultValue("Active");
				entity.Property(e => e.Title)
					.HasMaxLength(200)
					.IsUnicode(false);
			});

			modelBuilder.Entity<PropertyAmenity>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Property__3214EC071193879A");

				entity.ToTable("PropertyAmenities", "dbo");

				entity.Property(e => e.Name)
					.HasMaxLength(100)
					.IsUnicode(false);
			});

			modelBuilder.Entity<PropertyAmenityMap>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Property__3214EC073F93806D");

				entity.ToTable("PropertyAmenityMap", "dbo");

				entity.Property(e => e.CreatedDate).HasColumnType("datetime");
				entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
			});

			modelBuilder.Entity<PropertyCategory>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Property__3214EC07B26087DB");

				entity.ToTable("PropertyCategories", "dbo");

				entity.Property(e => e.Name).HasMaxLength(50);
			});

			modelBuilder.Entity<PropertyType>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Property__516F03B5F79FAECA");

				entity.ToTable("PropertyTypes", "dbo");

				entity.Property(e => e.Name).HasMaxLength(50);
			});

			modelBuilder.Entity<PropertyImage>(entity =>
			{
				entity.HasKey(e => e.ImageId).HasName("PK__Property__7516F70C9533DEE9");

				entity.Property(e => e.ImageUrl)
					.HasMaxLength(300)
					.IsUnicode(false);
				entity.Property(e => e.IsPrimary).HasDefaultValue(false);

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

		public int SaveChanges(CancellationToken cancellationToken = default)
		{
			var entities = (from entry in ChangeTracker.Entries()
							where (entry.State == EntityState.Modified || entry.State == EntityState.Added)
							//&& (entry.Entity.ToString() != (typeof(Doctor_Department_Mapping)).FullName)
							select entry).ToList();

			var user = Common.LoggedUser_Id();

			if (user == null || user <= 0)
				throw new InvalidOperationException("Opps...! An unexpected error occurred while saving.");
			else
			{
				foreach (var entity in entities)
				{
					if (entity.State == EntityState.Added)
					{
						((EntitiesBase)entity.Entity).IsActive = true;
						((EntitiesBase)entity.Entity).IsDeleted = false;
						((EntitiesBase)entity.Entity).CreatedDate = DateTime.Now;
						((EntitiesBase)entity.Entity).CreatedBy = ((EntitiesBase)entity.Entity).CreatedBy == 0 ? user : ((EntitiesBase)entity.Entity).CreatedBy;
						((EntitiesBase)entity.Entity).LastModifiedDate = DateTime.Now;
						((EntitiesBase)entity.Entity).LastModifiedBy = ((EntitiesBase)entity.Entity).CreatedBy == 0 ? user : ((EntitiesBase)entity.Entity).CreatedBy;
					}

					if (entity.State == EntityState.Modified)
					{
						((EntitiesBase)entity.Entity).LastModifiedDate = DateTime.Now;
						((EntitiesBase)entity.Entity).LastModifiedBy = user;
					}

					if (entity.State == EntityState.Deleted)
					{
						((EntitiesBase)entity.Entity).IsActive = false;
						((EntitiesBase)entity.Entity).IsDeleted = true;
						((EntitiesBase)entity.Entity).LastModifiedDate = DateTime.Now;
						((EntitiesBase)entity.Entity).LastModifiedBy = user;
					}

				}
			}

			return base.SaveChanges();
		}
	}


	public static class DataContext_Command
	{
		public static string _connectionString = AppHttpContextAccessor.AppConfiguration.GetSection("ConnectionStrings").GetSection("DataConnection").Value;

		public static string Get_DbSchemaName()
		{
			string keyValue = "database=";
			int startIndex = _connectionString.IndexOf(keyValue) + keyValue.Length;
			int endIndex = _connectionString.IndexOf(';', startIndex);
			return _connectionString.Substring(startIndex, endIndex - startIndex);
		}

		public static DataTable ExecuteQuery(string query)
		{
			try
			{
				DataTable dt = new DataTable();

				SqlConnection connection = new SqlConnection(_connectionString);

				SqlDataAdapter oraAdapter = new SqlDataAdapter(query, connection);

				oraAdapter.Fill(dt);

				return dt;
			}
			catch (Exception ex)
			{
				LogService.LogInsert("ExecuteQuery_DataTable - DataContext", "", ex);
				return null;
			}

		}

		public static DataSet ExecuteQuery_DataSet(string sqlquerys)
		{
			DataSet ds = new DataSet();

			try
			{
				DataTable dt = new DataTable();

				SqlConnection connection = new SqlConnection(_connectionString);

				foreach (var sqlquery in sqlquerys.Split(";"))
				{
					dt = new DataTable();

					SqlDataAdapter oraAdapter = new SqlDataAdapter(sqlquery, connection);

					SqlCommandBuilder oraBuilder = new SqlCommandBuilder(oraAdapter);

					oraAdapter.Fill(dt);

					if (dt != null)
						ds.Tables.Add(dt);
				}

			}
			catch (Exception ex)
			{
				LogService.LogInsert("ExecuteQuery_DataSet - DataContext", "", ex);
				return null;
			}

			return ds;
		}

		public static DataTable ExecuteStoredProcedure_DataTable(string query, List<SqlParameter> parameters = null, bool returnParameter = false)
		{
			DataTable dt = new DataTable();

			try
			{
				using (SqlConnection conn = new SqlConnection(_connectionString))
				{
					conn.Open();
					using (SqlCommand cmd = new SqlCommand(query, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;

						if (parameters != null)
							foreach (SqlParameter param in parameters)
								cmd.Parameters.Add(param);

						SqlDataAdapter da = new SqlDataAdapter(cmd);

						da.Fill(dt);

						parameters = null;
					}
					conn.Close();
				}
			}
			catch (Exception ex)
			{
				LogService.LogInsert("ExecuteStoredProcedure_DataTable - DataContext", "", ex);
				return null;
			}

			return dt;
		}

		public static DataSet ExecuteStoredProcedure_DataSet(string sp, List<SqlParameter> spCol = null)
		{
			DataSet ds = new DataSet();

			try
			{
				using (SqlConnection con = new SqlConnection(_connectionString))
				{
					con.Open();

					using (SqlCommand cmd = new SqlCommand(sp, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;

						if (spCol != null && spCol.Count > 0)
							cmd.Parameters.AddRange(spCol.ToArray());

						using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
						{
							adp.Fill(ds);
						}
					}

					con.Close();
				}
			}
			catch (Exception ex) { LogService.LogInsert("ExecuteStoredProcedure_DataSet - DataContext", "", ex); }

			return ds;
		}

		public static bool ExecuteNonQuery(string query, List<SqlParameter> parameters = null)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(_connectionString))
				{
					con.Open();

					SqlCommand cmd = con.CreateCommand();

					cmd.CommandType = CommandType.Text;
					cmd.CommandText = query;

					if (parameters != null)
						foreach (SqlParameter param in parameters)
							cmd.Parameters.Add(param);

					cmd.ExecuteNonQuery();
				}

				return true;
			}
			catch (Exception ex)
			{
				LogService.LogInsert("ExecuteNonQuery - DataContext", "", ex);
				return false;
			}
		}

		public static (bool, string, long) ExecuteStoredProcedure(string query, List<SqlParameter> parameters, bool returnParameter = false)
		{
			var response = string.Empty;

			using (SqlConnection con = new SqlConnection(_connectionString))
			{
				using (SqlCommand cmd = con.CreateCommand())
				{
					try
					{
						con.Open();

						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandText = query;
						//cmd.DeriveParameters();

						if (parameters != null && parameters.Count > 0)
							cmd.Parameters.AddRange(parameters.ToArray());

						if (returnParameter)
							cmd.Parameters.Add(new SqlParameter("@response", SqlDbType.VarChar, 2000) { Direction = ParameterDirection.Output });

						cmd.CommandTimeout = 86400;
						cmd.ExecuteNonQuery();

						//RETURN VALUE
						//response = cmd.Parameters["P_Response"].Value.ToString();

						response = "S|Success";

						if (cmd.Parameters.Contains("@response"))
						{
							response = cmd.Parameters["@response"].Value.ToString();
						}

						con.Close();
						cmd.Parameters.Clear();
						cmd.Dispose();

					}
					catch (Exception ex)
					{
						con.Close();
						cmd.Parameters.Clear();
						cmd.Dispose();

						response = "E|Opps!... Something went wrong. " + JsonConvert.SerializeObject(ex) + "|0";
					}
				}
			}

			if (!string.IsNullOrEmpty(response) && response.Contains("|"))
			{
				var msgtype = response.Split('|').Length > 0 ? Convert.ToString(response.Split('|')[0]) : "";
				var message = response.Split('|').Length > 1 ? Convert.ToString(response.Split('|')[1]).Replace("\"", "") : "";

				Int64 strid = 0;
				if (Int64.TryParse(response.Split('|').Length > 2 ? Convert.ToString(response.Split('|')[2]).Replace("\"", "") : "0", out strid)) { }
				//string paths = response.Split('|').Length > 3 ? response.Split('|')[3].Replace("\"", "") : "0";


				return (msgtype.Contains("S"), message, strid);
			}
			else
				return (false, ResponseStatusMessage.Error, 0);
		}


		public static (bool, string, long, string) ExecuteStoredProcedure_SQLwithpath(string query, List<SqlParameter> parameters, bool returnParameter = false)
		{
			var response = string.Empty;

			using (SqlConnection con = new SqlConnection(_connectionString))
			{
				using (SqlCommand cmd = con.CreateCommand())
				{
					try
					{
						con.Open();

						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandText = query;
						//cmd.DeriveParameters();

						if (parameters != null && parameters.Count > 0)
							cmd.Parameters.AddRange(parameters.ToArray());

						if (returnParameter)
							cmd.Parameters.Add(new SqlParameter("@response", SqlDbType.VarChar, 2000) { Direction = ParameterDirection.Output });

						cmd.CommandTimeout = 86400;
						cmd.ExecuteNonQuery();

						//RETURN VALUE
						//response = cmd.Parameters["P_Response"].Value.ToString();

						response = "S|Success";

						if (cmd.Parameters.Contains("@response"))
						{
							response = cmd.Parameters["@response"].Value.ToString();
						}

						con.Close();
						cmd.Parameters.Clear();
						cmd.Dispose();

					}
					catch (Exception ex)
					{
						con.Close();
						cmd.Parameters.Clear();
						cmd.Dispose();

						response = "E|Opps!... Something went wrong. " + JsonConvert.SerializeObject(ex) + "|0";
					}
				}
			}

			if (!string.IsNullOrEmpty(response) && response.Contains("|"))
			{
				var msgtype = response.Split('|').Length > 0 ? Convert.ToString(response.Split('|')[0]) : "";
				var message = response.Split('|').Length > 1 ? Convert.ToString(response.Split('|')[1]).Replace("\"", "") : "";

				Int64 strid = 0;
				if (Int64.TryParse(response.Split('|').Length > 2 ? Convert.ToString(response.Split('|')[2]).Replace("\"", "") : "0", out strid)) { }
				string paths = response.Split('|').Length > 3 ? response.Split('|')[3].Replace("\"", "") : "0";


				return (msgtype.Contains("S"), message, strid, paths);
			}
			else
				return (false, ResponseStatusMessage.Error, 0, "0");
		}

		public static string ExecuteStoredProcedure(string sp, SqlParameter[] spCol)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(_connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(sp, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;

						SqlParameter returnParameter = new SqlParameter("@response", SqlDbType.NVarChar, 2000);

						returnParameter.Direction = ParameterDirection.Output;

						if (spCol != null && spCol.Length > 0)
							cmd.Parameters.AddRange(spCol);


						cmd.Parameters.Add(returnParameter);

						conn.Open();
						cmd.ExecuteNonQuery();
						conn.Close();

						return returnParameter.Value.ToString();
					}
				}

			}
			catch (SqlException ex)
			{
				StringBuilder errorMessages = new StringBuilder();
				for (int i = 0; i < ex.Errors.Count; i++)
				{
					errorMessages.Append("Index #......" + i.ToString() + Environment.NewLine +
										 "Message:....." + ex.Errors[i].Message + Environment.NewLine +
										 "LineNumber:.." + ex.Errors[i].LineNumber + Environment.NewLine);
				}
				//Activity_Log.SendToDB("Database Oparation", "Error: " + "StoredProcedure: " + sp, ex);
				return "E|" + errorMessages.ToString();
			}
			catch (Exception ex)
			{
				//Activity_Log.SendToDB("Database Oparation", "Error: " + "StoredProcedure: " + sp, ex);
				return "E|" + ex.Message.ToString();
			}
		}

		public static bool ExecuteNonQuery_Delete(string query, List<SqlParameter> parameters = null)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(_connectionString))
				{
					con.Open();

					SqlCommand cmd = con.CreateCommand();
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = query;

					if (parameters != null)
						foreach (SqlParameter param in parameters)
							cmd.Parameters.Add(param);

					cmd.ExecuteNonQuery();
				}

				return true;
			}
			catch (Exception ex)
			{
				LogService.LogInsert("ExecuteNonQuery_Delete - DataContext", "", ex);
				return false;
			}
		}


		public static List<Employee> Employee_Get(long id = 0, long Logged_In_VendorId = 0)
		{
			DateTime? nullDateTime = null;
			var listObj = new List<Employee>();

			try
			{
				var parameters = new List<SqlParameter>();
				parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = id, Direction = ParameterDirection.Input, IsNullable = true });
				parameters.Add(new SqlParameter("VendorId", SqlDbType.BigInt) { Value = Logged_In_VendorId, Direction = ParameterDirection.Input, IsNullable = true });

				parameters.Add(new SqlParameter("Operated_By", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input, IsNullable = true });
				parameters.Add(new SqlParameter("Operated_RoleId", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ROLE_ID), Direction = ParameterDirection.Input, IsNullable = true });
				parameters.Add(new SqlParameter("Operated_MenuId", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.CURRENT_MENU_ID), Direction = ParameterDirection.Input, IsNullable = true });

				var dt = ExecuteStoredProcedure_DataTable("SP_Employee_GET", parameters.ToList());

				if (dt != null && dt.Rows.Count > 0)
					foreach (DataRow dr in dt.Rows)
						listObj.Add(new Employee()
						{
							Id = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : 0,
							RoleId = dr["RoleId"] != DBNull.Value ? Convert.ToInt64(dr["RoleId"]) : 0,
							UserId = dr["UserId"] != DBNull.Value ? Convert.ToInt64(dr["UserId"]) : 0,
							VendorId = dr["VendorId"] != DBNull.Value ? Convert.ToInt64(dr["VendorId"]) : 0,
							UserName = dr["UserName"] != DBNull.Value ? Convert.ToString(dr["UserName"]) : "",
							FirstName = dr["FirstName"] != DBNull.Value ? Convert.ToString(dr["FirstName"]) : "",
							MiddleName = dr["MiddleName"] != DBNull.Value ? Convert.ToString(dr["MiddleName"]) : "",
							LastName = dr["LastName"] != DBNull.Value ? Convert.ToString(dr["LastName"]) : "",
							UserType = dr["UserType"] != DBNull.Value ? Convert.ToString(dr["UserType"]) : "",
							BirthDate = dr["BirthDate"] != DBNull.Value ? Convert.ToDateTime(dr["BirthDate"]) : nullDateTime,
							BirthDate_Text = dr["BirthDate_Text"] != DBNull.Value ? Convert.ToString(dr["BirthDate_Text"]) : "",
							IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
							IsDeleted = dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : false,
							CreatedBy = dr["CreatedBy"] != DBNull.Value ? Convert.ToInt64(dr["CreatedBy"]) : 0
						});
			}
			catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

			return listObj;
		}

		public static (bool, string, long) Employee_Save(Employee obj = null)
		{
			if (obj != null)
				try
				{
					var parameters = new List<SqlParameter>();

					parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = obj.Id, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("UserId", SqlDbType.BigInt) { Value = obj.UserId, Direction = ParameterDirection.Input, IsNullable = true });
					//parameters.Add(new SqlParameter("RoleId", SqlDbType.BigInt) { Value = obj.RoleId, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("VendorId", SqlDbType.BigInt) { Value = obj.VendorId, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("UserName", SqlDbType.NVarChar) { Value = obj.UserName, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Password", SqlDbType.NVarChar) { Value = obj.Password, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("FirstName", SqlDbType.NVarChar) { Value = obj.FirstName, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("MiddleName", SqlDbType.NVarChar) { Value = obj.MiddleName, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("LastName", SqlDbType.NVarChar) { Value = obj.LastName, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("UserType", SqlDbType.NVarChar) { Value = obj.UserType, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("BirthDate", SqlDbType.NVarChar) { Value = obj.BirthDate?.ToString("dd/MM/yyyy"), Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("IsActive", SqlDbType.NVarChar) { Value = obj.IsActive, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Operated_By", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Operated_RoleId", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ROLE_ID), Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Operated_MenuId", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.CURRENT_MENU_ID), Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Action", SqlDbType.NVarChar) { Value = obj.Id > 0 ? "UPDATE" : "INSERT", Direction = ParameterDirection.Input, IsNullable = true });

					var response = ExecuteStoredProcedure("SP_Employee_Save", parameters.ToArray());

					var msgtype = response.Split('|').Length > 0 ? response.Split('|')[0] : "";
					var message = response.Split('|').Length > 1 ? response.Split('|')[1].Replace("\"", "") : "";
					var strid = response.Split('|').Length > 2 ? response.Split('|')[2].Replace("\"", "") ?? "0" : "0";

					return (msgtype.Contains("S"), message, Convert.ToInt64(strid));

				}
				catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

			return (false, ResponseStatusMessage.Error, 0);
		}

		public static List<PropertyCategory> PropertyCategory_Get(long id = 0)
		{
			DateTime? nullDateTime = null;
			var listObj = new List<PropertyCategory>();

			try
			{
				var parameters = new List<SqlParameter>();
				parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = id, Direction = ParameterDirection.Input, IsNullable = true });

				var dt = ExecuteStoredProcedure_DataTable("SP_PropertyCategories_Get", parameters.ToList());

				if (dt != null && dt.Rows.Count > 0)
					foreach (DataRow dr in dt.Rows)
						listObj.Add(new PropertyCategory()
						{
							Id = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : 0,
							Name = dr["Name"] != DBNull.Value ? Convert.ToString(dr["Name"]) : "",
							IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false
						});
			}
			catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

			return listObj;
		}

		public static List<PropertyType> PropertyType_Get(long id = 0)
		{
			DateTime? nullDateTime = null;
			var listObj = new List<PropertyType>();

			try
			{
				var parameters = new List<SqlParameter>();
				parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = id, Direction = ParameterDirection.Input, IsNullable = true });

				var dt = ExecuteStoredProcedure_DataTable("SP_PropertyTypes_Get", parameters.ToList());

				if (dt != null && dt.Rows.Count > 0)
					foreach (DataRow dr in dt.Rows)
						listObj.Add(new PropertyType()
						{
							Id = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : 0,
							ParentId = dr["ParentId"] != DBNull.Value ? Convert.ToInt64(dr["ParentId"]) : 0,
							Name = dr["Name"] != DBNull.Value ? Convert.ToString(dr["Name"]) : "",
							IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false
						});
			}
			catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

			return listObj;
		}

		public static List<Properties> Property_Get(long id = 0)
		{
			DateTime? nullDateTime = null;
			var listObj = new List<Properties>();

			try
			{
				var parameters = new List<SqlParameter>();
				parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = id, Direction = ParameterDirection.Input, IsNullable = true });

				var dt = ExecuteStoredProcedure_DataTable("SP_Property_Get", parameters.ToList());

				if (dt != null && dt.Rows.Count > 0)
					foreach (DataRow dr in dt.Rows)
						listObj.Add(new Properties()
						{
							Id = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : 0,
							Title = dr["Title"] != DBNull.Value ? Convert.ToString(dr["Title"]) : "",
							Description = dr["Description"] != DBNull.Value ? Convert.ToString(dr["Description"]) : "",
							CityId = dr["CityId"] != DBNull.Value ? Convert.ToInt64(dr["CityId"]) : 0,
							AreaId = dr["AreaId"] != DBNull.Value ? Convert.ToInt64(dr["AreaId"]) : 0,
							Landmark = dr["Landmark"] != DBNull.Value ? Convert.ToString(dr["Landmark"]) : "",
							CategoryId = dr["CategoryId"] != DBNull.Value ? Convert.ToInt64(dr["CategoryId"]) : 0,
							TypeId = dr["TypeId"] != DBNull.Value ? Convert.ToInt64(dr["TypeId"]) : 0,
							Property_Type = dr["Property_Type"] != DBNull.Value ? Convert.ToString(dr["Property_Type"]) : "",
							Property_Category = dr["Property_Category"] != DBNull.Value ? Convert.ToString(dr["Property_Category"]) : "",
							Price = dr["Price"] != DBNull.Value ? Convert.ToDecimal(dr["Price"]) : 0,
							AreaSqft = dr["AreaSqft"] != DBNull.Value ? Convert.ToDecimal(dr["AreaSqft"]) : 0,
							OwnerName = dr["OwnerName"] != DBNull.Value ? Convert.ToString(dr["OwnerName"]) : "",
							OwnerMobile = dr["OwnerMobile"] != DBNull.Value ? Convert.ToString(dr["OwnerMobile"]) : "",
							BuilderName = dr["BuilderName"] != DBNull.Value ? Convert.ToString(dr["BuilderName"]) : "",
							FloorNo = dr["FloorNo"] != DBNull.Value ? Convert.ToInt32(dr["FloorNo"]) : 0,
							TotalFloors = dr["TotalFloors"] != DBNull.Value ? Convert.ToInt32(dr["TotalFloors"]) : 0,
							Facing = dr["Facing"] != DBNull.Value ? Convert.ToString(dr["Facing"]) : "",
							FurnishingStatus = dr["FurnishingStatus"] != DBNull.Value ? Convert.ToString(dr["FurnishingStatus"]) : "",
							FurnishingStatus_TEXT = dr["FurnishingStatus_TEXT"] != DBNull.Value ? Convert.ToString(dr["FurnishingStatus_TEXT"]) : "",
							AvailabilityStatus = dr["AvailabilityStatus"] != DBNull.Value ? Convert.ToString(dr["AvailabilityStatus"]) : "",
							AvailabilityStatus_TEXT = dr["AvailabilityStatus_TEXT"] != DBNull.Value ? Convert.ToString(dr["AvailabilityStatus_TEXT"]) : "",
							Remark = dr["Remark"] != DBNull.Value ? Convert.ToString(dr["Remark"]) : "",
							IsFeatured = dr["IsFeatured"] != DBNull.Value ? Convert.ToBoolean(dr["IsFeatured"]) : false,
							IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false
						});
			}
			catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

			return listObj;
		}

        public static List<Lead> Lead_Get(long id = 0)
        {
            DateTime? nullDateTime = null;
            var listObj = new List<Lead>();

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = id, Direction = ParameterDirection.Input, IsNullable = true });

                var dt = ExecuteStoredProcedure_DataTable("SP_Leads_Get", parameters.ToList());

                if (dt != null && dt.Rows.Count > 0)
                    foreach (DataRow dr in dt.Rows)
                        listObj.Add(new Lead()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : 0,
                            Name = dr["Name"] != DBNull.Value ? Convert.ToString(dr["Name"]) : "",
                            Mobile = dr["Mobile"] != DBNull.Value ? Convert.ToString(dr["Mobile"]) : "",
                            Email = dr["Email"] != DBNull.Value ? Convert.ToString(dr["Email"]) : "",
                            Requirement = dr["Requirement"] != DBNull.Value ? Convert.ToString(dr["Requirement"]) : "",
                            Preferred_City_Id = dr["Preferred_City_Id"] != DBNull.Value ? Convert.ToInt64(dr["Preferred_City_Id"]) : 0,
                            Preferred_Area_Id = dr["Preferred_Area_Id"] != DBNull.Value ? Convert.ToInt64(dr["Preferred_Area_Id"]) : 0,
                            Landmark = dr["Landmark"] != DBNull.Value ? Convert.ToString(dr["Landmark"]) : "",
                            AssignedTo_Text = dr["AssignedTo_Text"] != DBNull.Value ? Convert.ToString(dr["AssignedTo_Text"]) : "",
                            AssignedTo = dr["AssignedTo"] != DBNull.Value ? Convert.ToInt64(dr["AssignedTo"]) : 0,
                            PropertyType = dr["PropertyType"] != DBNull.Value ? Convert.ToInt64(dr["PropertyType"]) : 0,
                            Property_Type_TEXT = dr["Property_Type_TEXT"] != DBNull.Value ? Convert.ToString(dr["Property_Type_TEXT"]) : "",
                            LeadSource_Value = dr["LeadSource"] != DBNull.Value ? Convert.ToString(dr["LeadSource"]) : "",
                            LeadSource_TEXT = dr["LeadSource_TEXT"] != DBNull.Value ? Convert.ToString(dr["LeadSource_TEXT"]) : "",
                            Status = dr["Status"] != DBNull.Value ? Convert.ToString(dr["Status"]) : "",
                            Status_TEXT = dr["Status_TEXT"] != DBNull.Value ? Convert.ToString(dr["Status_TEXT"]) : "",
                            BudgetMin = dr["BudgetMin"] != DBNull.Value ? Convert.ToDecimal(dr["BudgetMin"]) : 0,
                            BudgetMax = dr["BudgetMax"] != DBNull.Value ? Convert.ToDecimal(dr["BudgetMax"]) : 0,
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false
                        });
            }
            catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

            return listObj;
        }

        public static List<LeadFollowup> LeadFollowUp_Get(long id = 0, long LeadId = 0 , string Status = "")
        {
            DateTime? nullDateTime = null;
            var listObj = new List<LeadFollowup>();

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = id, Direction = ParameterDirection.Input, IsNullable = true });
                parameters.Add(new SqlParameter("LeadId", SqlDbType.BigInt) { Value = LeadId, Direction = ParameterDirection.Input, IsNullable = true });

                var dt = ExecuteStoredProcedure_DataTable("SP_LeadFollowups_Get", parameters.ToList());

                if (dt != null && dt.Rows.Count > 0)
                    foreach (DataRow dr in dt.Rows)
                        listObj.Add(new LeadFollowup()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : 0,
                            LeadId = dr["LeadId"] != DBNull.Value ? Convert.ToInt64(dr["LeadId"]) : 0,
                            NextFollowupDate = dr["NextFollowupDate"] != DBNull.Value ? Convert.ToDateTime(dr["NextFollowupDate"]) : nullDateTime,
                            ReminderDatetime = dr["ReminderDatetime"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDatetime"]) : nullDateTime,
                            Remark = dr["Remark"] != DBNull.Value ? Convert.ToString(dr["Remark"]) : "",
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                            Status = Status
                        });
            }
            catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

            return listObj;
        }
        public static (bool, string, long) Leads_Save(Lead obj = null)
        {
            if (obj != null)
                try
                {
                    var parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = obj.Id, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Name", SqlDbType.VarChar) { Value = obj.Name, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Mobile", SqlDbType.VarChar) { Value = obj.Mobile, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Email", SqlDbType.VarChar) { Value = obj.Email, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Requirement", SqlDbType.VarChar) { Value = obj.Requirement, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Preferred_City_Id", SqlDbType.BigInt) { Value = obj.Preferred_City_Id, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Preferred_Area_Id", SqlDbType.BigInt) { Value = obj.Preferred_Area_Id, Direction = ParameterDirection.Input, IsNullable = true });
                    //parameters.Add(new SqlParameter("AssignedTo", SqlDbType.BigInt) { Value = obj.AssignedTo, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("PropertyType", SqlDbType.BigInt) { Value = obj.PropertyType, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Landmark", SqlDbType.VarChar) { Value = obj.Landmark, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("LeadSource", SqlDbType.VarChar) { Value = obj.LeadSource_Value, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("BudgetMin", SqlDbType.Decimal) { Value = obj.BudgetMin, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("BudgetMax", SqlDbType.Decimal) { Value = obj.BudgetMax, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Operated_By", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Action", SqlDbType.NVarChar) { Value = obj.Id > 0 ? "UPDATE" : "INSERT", Direction = ParameterDirection.Input, IsNullable = true });

                    var response = ExecuteStoredProcedure("SP_Leads_Save", parameters.ToArray());

                    var msgtype = response.Split('|').Length > 0 ? response.Split('|')[0] : "";
                    var message = response.Split('|').Length > 1 ? response.Split('|')[1].Replace("\"", "") : "";
                    var strid = response.Split('|').Length > 2 ? response.Split('|')[2].Replace("\"", "") ?? "0" : "0";

                    return (msgtype.Contains("S"), message, Convert.ToInt64(strid));

                }
                catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

            return (false, ResponseStatusMessage.Error, 0);
        }

        public static (bool, string, long) LeadFollowUp_Save(LeadFollowup obj = null)
        {
            if (obj != null)
                try
                {
                    var parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = obj.Id, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("LeadId", SqlDbType.BigInt) { Value = obj.LeadId, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("NextFollowupDate", SqlDbType.DateTime) { Value = obj.NextFollowupDate, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("ReminderDatetime", SqlDbType.DateTime) { Value = obj.ReminderDatetime, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Remark", SqlDbType.VarChar) { Value = obj.Remark, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Operated_By", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Action", SqlDbType.NVarChar) { Value = obj.Id > 0 ? "UPDATE" : "INSERT", Direction = ParameterDirection.Input, IsNullable = true });

                    var response = ExecuteStoredProcedure("SP_LeadFollowups_Save", parameters.ToArray());

                    var msgtype = response.Split('|').Length > 0 ? response.Split('|')[0] : "";
                    var message = response.Split('|').Length > 1 ? response.Split('|')[1].Replace("\"", "") : "";
                    var strid = response.Split('|').Length > 2 ? response.Split('|')[2].Replace("\"", "") ?? "0" : "0";

                    return (msgtype.Contains("S"), message, Convert.ToInt64(strid));

                }
                catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

            return (false, ResponseStatusMessage.Error, 0);
        }
        public static List<PropertyType> Property_Sub_Type_Get(long id = 0, long Parent_Id = 0)
		{
			DateTime? nullDateTime = null;
			var listObj = new List<PropertyType>();
			  
			try
			{
				var parameters = new List<SqlParameter>();
				parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = id, Direction = ParameterDirection.Input, IsNullable = true });
				parameters.Add(new SqlParameter("Parent_Id", SqlDbType.BigInt) { Value = Parent_Id, Direction = ParameterDirection.Input, IsNullable = true });

				var dt = ExecuteStoredProcedure_DataTable("SP_Property_Sub_Type_Get", parameters.ToList());

				if (dt != null && dt.Rows.Count > 0)
					foreach (DataRow dr in dt.Rows)
						listObj.Add(new PropertyType()
						{
							Id = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : 0,
							ParentId = dr["ParentId"] != DBNull.Value ? Convert.ToInt64(dr["ParentId"]) : 0,
							Name = dr["Name"] != DBNull.Value ? Convert.ToString(dr["Name"]) : "",
							IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false
						});
			}
			catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

			return listObj;
		}

		public static (bool, string, long) PropertyCategory_Save(PropertyCategory obj = null)
		{
			if (obj != null)
				try
				{
					var parameters = new List<SqlParameter>();

					parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = obj.Id, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Name", SqlDbType.VarChar) { Value = obj.Name, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("IsActive", SqlDbType.NVarChar) { Value = obj.IsActive, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Operated_By", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Action", SqlDbType.NVarChar) { Value = obj.Id > 0 ? "UPDATE" : "INSERT", Direction = ParameterDirection.Input, IsNullable = true });

					var response = ExecuteStoredProcedure("SP_PropertyCategories_Save", parameters.ToArray());

					var msgtype = response.Split('|').Length > 0 ? response.Split('|')[0] : "";
					var message = response.Split('|').Length > 1 ? response.Split('|')[1].Replace("\"", "") : "";
					var strid = response.Split('|').Length > 2 ? response.Split('|')[2].Replace("\"", "") ?? "0" : "0";

					return (msgtype.Contains("S"), message, Convert.ToInt64(strid));

				}
				catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

			return (false, ResponseStatusMessage.Error, 0);
		}
		public static (bool, string) PropertyCategory_Delete(long Id = 0)
		{
			if (Id > 0)
				try
				{
					var parameters = new List<SqlParameter>();

					parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = Id, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Operated_By", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input, IsNullable = true });

					var response = ExecuteStoredProcedure("sp_PropertyCategories_Delete", parameters.ToArray());

					var msgtype = response.Split('|').Length > 0 ? response.Split('|')[0] : "";
					var message = response.Split('|').Length > 1 ? response.Split('|')[1].Replace("\"", "") : "";
					var strid = response.Split('|').Length > 2 ? response.Split('|')[2].Replace("\"", "") ?? "0" : "0";

					return (msgtype.Contains("S"), message);

				}
				catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

			return (false, ResponseStatusMessage.Error);
		}

		public static (bool, string, long) PropertyType_Save(PropertyType obj = null)
		{
			if (obj != null)
				try
				{
					var parameters = new List<SqlParameter>();

					parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = obj.Id, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Parent_Id", SqlDbType.BigInt) { Value = obj.ParentId, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Name", SqlDbType.VarChar) { Value = obj.Name, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("IsActive", SqlDbType.NVarChar) { Value = obj.IsActive, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Operated_By", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Action", SqlDbType.NVarChar) { Value = obj.Id > 0 ? "UPDATE" : "INSERT", Direction = ParameterDirection.Input, IsNullable = true });

					var response = ExecuteStoredProcedure("SP_PropertyTypes_Save", parameters.ToArray());

					var msgtype = response.Split('|').Length > 0 ? response.Split('|')[0] : "";
					var message = response.Split('|').Length > 1 ? response.Split('|')[1].Replace("\"", "") : "";
					var strid = response.Split('|').Length > 2 ? response.Split('|')[2].Replace("\"", "") ?? "0" : "0";

					return (msgtype.Contains("S"), message, Convert.ToInt64(strid));

				}
				catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

			return (false, ResponseStatusMessage.Error, 0);
		}

		public static (bool, string, long) Property_Save(Properties obj = null)
		{
			if (obj != null)
				try
				{
					var parameters = new List<SqlParameter>();

					parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = obj.Id, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Title", SqlDbType.VarChar) { Value = obj.Title, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Description", SqlDbType.VarChar) { Value = obj.Description, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("CityId", SqlDbType.BigInt) { Value = obj.CityId, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("AreaId", SqlDbType.BigInt) { Value = obj.AreaId, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("CategoryId", SqlDbType.BigInt) { Value = obj.CategoryId, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("TypeId", SqlDbType.BigInt) { Value = obj.TypeId, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Landmark", SqlDbType.VarChar) { Value = obj.Landmark, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Price", SqlDbType.Decimal) { Value = obj.Price, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("AreaSqft", SqlDbType.Decimal) { Value = obj.AreaSqft, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("OwnerName", SqlDbType.VarChar) { Value = obj.OwnerName, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("OwnerMobile", SqlDbType.VarChar) { Value = obj.OwnerMobile, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("BuilderName", SqlDbType.VarChar) { Value = obj.BuilderName, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("FloorNo", SqlDbType.Int) { Value = obj.FloorNo, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("TotalFloors", SqlDbType.Int) { Value = obj.TotalFloors, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Facing", SqlDbType.VarChar) { Value = obj.Facing, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("FurnishingStatus", SqlDbType.VarChar) { Value = obj.FurnishingStatus, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("IsFeatured", SqlDbType.Bit) { Value = obj.IsFeatured, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Remark", SqlDbType.VarChar) { Value = obj.Remark, Direction = ParameterDirection.Input, IsNullable = true });
					//parameters.Add(new SqlParameter("IsActive", SqlDbType.Bit) { Value = obj.IsActive, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Operated_By", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Action", SqlDbType.NVarChar) { Value = obj.Id > 0 ? "UPDATE" : "INSERT", Direction = ParameterDirection.Input, IsNullable = true });

					var response = ExecuteStoredProcedure("SP_Property_Save", parameters.ToArray());

					var msgtype = response.Split('|').Length > 0 ? response.Split('|')[0] : "";
					var message = response.Split('|').Length > 1 ? response.Split('|')[1].Replace("\"", "") : "";
					var strid = response.Split('|').Length > 2 ? response.Split('|')[2].Replace("\"", "") ?? "0" : "0";

					return (msgtype.Contains("S"), message, Convert.ToInt64(strid));

				}
				catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

			return (false, ResponseStatusMessage.Error, 0);
		}
		public static (bool, string) PropertyType_Delete(long Id = 0, long ParentId = 0)
		{
			if (Id > 0)
				try
				{
					var parameters = new List<SqlParameter>();

					parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = Id, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Parent_Id", SqlDbType.BigInt) { Value = ParentId, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Operated_By", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input, IsNullable = true });

					var response = ExecuteStoredProcedure("sp_PropertyTypes_Delete", parameters.ToArray());

					var msgtype = response.Split('|').Length > 0 ? response.Split('|')[0] : "";
					var message = response.Split('|').Length > 1 ? response.Split('|')[1].Replace("\"", "") : "";
					var strid = response.Split('|').Length > 2 ? response.Split('|')[2].Replace("\"", "") ?? "0" : "0";

					return (msgtype.Contains("S"), message);

				}
				catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

			return (false, ResponseStatusMessage.Error);
		}

        public static (bool, string) Lead_Status_Change(long Id = 0, string Remarks = "" , string Status = "")
        {
            if (Id > 0)
                try
                {
                    var parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter("LeadId", SqlDbType.BigInt) { Value = Id, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Status", SqlDbType.VarChar) { Value = Status, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Remarks", SqlDbType.VarChar) { Value = Remarks, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Operated_By", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input, IsNullable = true });

                    var response = ExecuteStoredProcedure("SP_LeadFollowUp_Status_Change", parameters.ToArray());

                    var msgtype = response.Split('|').Length > 0 ? response.Split('|')[0] : "";
                    var message = response.Split('|').Length > 1 ? response.Split('|')[1].Replace("\"", "") : "";
                    var strid = response.Split('|').Length > 2 ? response.Split('|')[2].Replace("\"", "") ?? "0" : "0";

                    return (msgtype.Contains("S"), message);

                }
                catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

            return (false, ResponseStatusMessage.Error);
        }
        public static (bool, string) Employee_Status(long Id = 0, long Logged_In_VendorId = 0, bool IsActive = false, bool IsDelete = false)
		{
			if (Id > 0)
				try
				{
					var parameters = new List<SqlParameter>();

					parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = Id, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("VendorId", SqlDbType.BigInt) { Value = Logged_In_VendorId, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("IsActive", SqlDbType.NVarChar) { Value = IsActive, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Operated_By", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Operated_RoleId", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ROLE_ID), Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Operated_MenuId", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.CURRENT_MENU_ID), Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("Action", SqlDbType.NVarChar) { Value = IsDelete ? "DELETE" : "STATUS", Direction = ParameterDirection.Input, IsNullable = true });

					var response = ExecuteStoredProcedure("SP_Employee_Status", parameters.ToArray());

					var msgtype = response.Split('|').Length > 0 ? response.Split('|')[0] : "";
					var message = response.Split('|').Length > 1 ? response.Split('|')[1].Replace("\"", "") : "";
					var strid = response.Split('|').Length > 2 ? response.Split('|')[2].Replace("\"", "") ?? "0" : "0";

					return (msgtype.Contains("S"), message);

				}
				catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

			return (false, ResponseStatusMessage.Error);
		}


        public static List<ServicesMaster> ServicesMaster_Get(long id = 0)
        {
            DateTime? nullDateTime = null;
            var listObj = new List<ServicesMaster>();

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("ServiceId", SqlDbType.BigInt) { Value = id, Direction = ParameterDirection.Input, IsNullable = true });

                var dt = ExecuteStoredProcedure_DataTable("SP_ServiceMaster_Get", parameters.ToList());

                if (dt != null && dt.Rows.Count > 0)
                    foreach (DataRow dr in dt.Rows)
                        listObj.Add(new ServicesMaster()
                        {
                            ServiceId = dr["ServiceId"] != DBNull.Value ? Convert.ToInt64(dr["ServiceId"]) : 0,
                            ServiceTitle = dr["ServiceTitle"] != DBNull.Value ? Convert.ToString(dr["ServiceTitle"]) : "",
                            ShortDescription = dr["ShortDescription"] != DBNull.Value ? Convert.ToString(dr["ShortDescription"]) : "",
                            FullDescription = dr["FullDescription"] != DBNull.Value ? Convert.ToString(dr["FullDescription"]) : "",
                            ImageName = dr["ImageName"] != DBNull.Value ? Convert.ToString(dr["ImageName"]) : "",
                            DisplayOrder = dr["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(dr["DisplayOrder"]) : 0,
                            IsFeatured = dr["IsFeatured"] != DBNull.Value ? Convert.ToBoolean(dr["IsFeatured"]) : false,
                            ResumeFile = dr["ResumeFile"] != DBNull.Value ? (byte[])dr["ResumeFile"] : null,
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false
                        });
            }
            catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

            return listObj;
        }


        public static (bool, string, long) ServicesMaster_Save(ServicesMaster obj = null)
        {
            if (obj != null)
                try
                {
                    var parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter("ServiceId", SqlDbType.BigInt) { Value = obj.ServiceId, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("ServiceTitle", SqlDbType.VarChar) { Value = obj.ServiceTitle, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("ShortDescription", SqlDbType.VarChar) { Value = obj.ShortDescription, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("FullDescription", SqlDbType.VarChar) { Value = obj.FullDescription, Direction = ParameterDirection.Input, IsNullable = true });

                    parameters.Add(new SqlParameter("ImageName", SqlDbType.VarChar, 255)
                    {
                        Value = (object)obj.ImageName ?? DBNull.Value
                    });

                    parameters.Add(new SqlParameter("ResumeFile", SqlDbType.VarBinary)
                    {
                        Value = (object)obj.ResumeFile ?? DBNull.Value
                    });

                    parameters.Add(new SqlParameter("DisplayOrder", SqlDbType.Int) { Value = obj.DisplayOrder, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("IsFeatured", SqlDbType.Bit)
                    {
                        Value = obj.IsFeatured
                    });
                    //parameters.Add(new SqlParameter("IsActive", SqlDbType.NVarChar) { Value = obj.IsActive, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Operated_By", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Action", SqlDbType.NVarChar) { Value = obj.ServiceId > 0 ? "UPDATE" : "INSERT", Direction = ParameterDirection.Input, IsNullable = true });

                    var response = ExecuteStoredProcedure("SP_ServicesMaster_Save", parameters.ToArray());

                    var msgtype = response.Split('|').Length > 0 ? response.Split('|')[0] : "";
                    var message = response.Split('|').Length > 1 ? response.Split('|')[1].Replace("\"", "") : "";
                    var strid = response.Split('|').Length > 2 ? response.Split('|')[2].Replace("\"", "") ?? "0" : "0";

                    return (msgtype.Contains("S"), message, Convert.ToInt64(strid));

                }
                catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

            return (false, ResponseStatusMessage.Error, 0);
        }


        public static (bool, string) ServicesMaster_Delete(long Id = 0)
        {
            if (Id > 0)
                try
                {
                    var parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter("ServiceId", SqlDbType.BigInt) { Value = Id, Direction = ParameterDirection.Input, IsNullable = true });
                    parameters.Add(new SqlParameter("Operated_By", SqlDbType.BigInt) { Value = Common.Get_Session_Int(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input, IsNullable = true });

                    var response = ExecuteStoredProcedure("sp_ServicesMaster_Delete", parameters.ToArray());

                    var msgtype = response.Split('|').Length > 0 ? response.Split('|')[0] : "";
                    var message = response.Split('|').Length > 1 ? response.Split('|')[1].Replace("\"", "") : "";
                    var strid = response.Split('|').Length > 2 ? response.Split('|')[2].Replace("\"", "") ?? "0" : "0";

                    return (msgtype.Contains("S"), message);

                }
                catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

            return (false, ResponseStatusMessage.Error);
        }

    }

}
