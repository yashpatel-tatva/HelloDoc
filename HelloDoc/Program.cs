using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using HelloDoc.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;


 void ConfigureServices(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Example API",
            Version = "v1",
            Description = "An example of an ASP.NET Core Web API",
            Contact = new OpenApiContact
            {
                Name = "Example Contact",
                Email = "example@example.com",
                Url = new Uri("https://example.com/contact"),
            },
        });
    });
    // Other service configurations...
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<HelloDocDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAspNetUserRepository, AspNetUserRepository>();
builder.Services.AddScoped<IRequestRepository , RequestRepository>();

var app = builder.Build();

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
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Patient}/{controller=Home}/{action=Index}/{id?}");

app.Run();
