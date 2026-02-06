using Broker.Infra;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


		var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAuthentication();
        // ✅ FIXED DbContext
        builder.Services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DataConnection"))
                );

        builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Limits.MaxRequestLineSize = 16 * 1024;
            options.Limits.MaxRequestHeadersTotalSize = 32 * 1024;
        });

        builder.Services.Configure<FormOptions>(o =>
        {
            o.ValueCountLimit = int.MaxValue;
            o.ValueLengthLimit = int.MaxValue;
            o.MultipartBodyLengthLimit = long.MaxValue;
        });

        builder.Services.AddControllersWithViews()
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
        builder.Services.Configure<RequestLocalizationOptions>(options =>
		{
			var cultureInfo = new CultureInfo("en-IN");
			cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
			cultureInfo.DateTimeFormat.LongDatePattern = "dd/MM/yyyy HH:mm:ss";

			var supportedCultures = new List<CultureInfo> { cultureInfo };

			options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-IN");

			options.DefaultRequestCulture.Culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
			options.DefaultRequestCulture.Culture.DateTimeFormat.LongDatePattern = "dd/MM/yyyy HH:mm:ss";

			options.SupportedCultures = supportedCultures;
			options.SupportedUICultures = supportedCultures;
		});

        // ✅ REQUIRED for FTP hosting
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto;
        });

// ✅ SQL session
                    builder.Services.AddDistributedSqlServerCache(options =>
                    {
                        options.ConnectionString =
                            builder.Configuration.GetConnectionString("DataConnection");
                        options.SchemaName = "dbo";
                        options.TableName = "AppSessions";
                    });

                    builder.Services.AddSession(options =>
                    {
                        options.IdleTimeout = TimeSpan.FromDays(1);
                        options.Cookie.HttpOnly = true;
                        options.Cookie.IsEssential = true;

                        options.Cookie.Name = ".Broker.Session";

                        // 🔥 IMPORTANT FIXES
                        options.Cookie.Path = "/";
                        options.Cookie.Domain = "vd1.padhyasoft.com";

                        options.Cookie.SameSite = SameSiteMode.Lax;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    });






var app = builder.Build();

		AppHttpContextAccessor.Configure(((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IHttpContextAccessor>(), ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IHostEnvironment>(), builder.Environment, ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IDataProtectionProvider>(), ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IConfiguration>(), ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IHttpClientFactory>());
        
        app.UseForwardedHeaders();
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}





        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

             // ✅ MUST be BEFORE Authentication
        app.UseAuthentication();
        app.UseSession();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
   