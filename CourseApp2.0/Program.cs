using CourseApp2._0.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using CourseApp2._0;
using Microsoft.Extensions.Options;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//CourseAppDBconnection and DefaultConnection

var connectionString = builder.Configuration.GetConnectionString("CourseAppDBconnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();





// This loads the OAuth 2.0 client ID used by this application from a client ID json file.
// You can use any mechanism you want to store and retrieve your client ID information, as long
// as it is secured. If your client ID information is leaked any other app can pose as your own.

ClientInfo clientInfo = ClientInfo.Load();

// This configures Google.Apis.Auth.AspNetCore3 for use in this app.
builder.Services
    .AddAuthentication()
    .AddCookie()
    .AddGoogle(GoogleDefaults.AuthenticationScheme, o =>
    {
        o.ClientId = clientInfo.ClientId;
        o.ClientSecret = clientInfo.ClientSecret;
    });
    





builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60);
    options.ExcludedHosts.Add("https://courseapp2020250402024208.azurewebsites.net");
    options.ExcludedHosts.Add("https://courseapp2020250402024208.azurewebsites.net/Samples");
    options.ExcludedHosts.Add("https://courseapp2020250402024208.azurewebsites.net/Search");
});


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

var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
};


app.UseStatusCodePagesWithRedirects("/Shared/{Error}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
