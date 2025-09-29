using Demo.BLL.Common.Services.Attachments;
using Demo.BLL.Common.Services.EmailSettings;
using Demo.BLL.Services.Departments;
using Demo.BLL.Services.Employee;
using Demo.BLL.Services.Employees;
using Demo.DAL.Entities.Identity;
using Demo.DAL.Persistence.Data;
using Demo.DAL.Persistence.UnitOfWork;
using Demo.PL.Mapping.Profiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            /// builder.Services.AddScoped<ApplicationDbContext>();
            /// builder.Services.AddScoped<DbContextOptions<ApplicationDbContext>>((ServiceProvider) =>
            /// {
            ///     var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            ///     optionBuilder.UseSqlServer("Server =.; Database = MVCApplication; Trusted_Connection = True; TrustServerCertificate = True;");
            ///     var options = optionBuilder.Options;
            ///     return options;
            /// });

            builder.Services.AddDbContext<ApplicationDbContext>((optionBuilder) =>
            {
                optionBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            builder.Services.AddScoped<IDepartmentService, DepartmentsServices>();

            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddTransient<IAttachmentService, AttachmentService>();

            builder.Services.AddScoped<IEmailSetting,  EmailSetting>();

            //builder.Services.AddScoped<UserManager<ApplicationUser>>();    //For Registeration
            //builder.Services.AddScoped<SignInManager<ApplicationUser>>();  //For Login
            //builder.Services.AddScoped<RoleManager<IdentityRole>>();       //For Authorization

            //Add Interfaces that CreateAsync Needed (Signture for methods)
            //Repositories ==> Stores
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>((options =>
            {
                options.Password.RequireLowercase = true;    //Configurations for password
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;   //@, #
                options.Password.RequiredLength = 5;
            }))
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders(); //PasswordSignInAsync Depend on AddDefaultTokenProviders Service

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(   //Add Three Services(UserManager, SignInManager, RoleManager) inside Dependency Injection Container
                options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Home/Error";
                    options.LogoutPath = "/Account/Login";
                });

            var app = builder.Build();

            #endregion

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Register}/{id?}");

            app.Run();
        }
    }
}
