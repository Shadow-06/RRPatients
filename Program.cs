using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RRPatients.Data;
using RRPatients.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var PatientsconnectionString = builder.Configuration.GetConnectionString("PatientsConnection");
builder.Services.AddDbContext<PatientsContext>(options =>
    options.UseSqlServer(PatientsconnectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSession();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "mydefault",
    pattern: "{action=Index}/{id?}",
    defaults: new { controller = "RRCountry" });

app.MapRazorPages();

app.Run();
