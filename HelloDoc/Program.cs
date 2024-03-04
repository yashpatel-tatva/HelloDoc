using DataAccess.Repository.IRepository;
using DataAccess.Repository;
using HelloDoc.DataContext;
using DataAccess.ServiceRepository.IServiceRepository;
using DataAccess.ServiceRepository;
using NPOI.Util;
using static HelloDoc.Areas.PatientArea.ViewModels.PatientDashboardViewModel;
using NPOI.SS.Formula.Functions;

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
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IBlockCaseRepository, BlockCaseRepository>();
builder.Services.AddScoped<IRequestStatusLogRepository , RequestStatusLogRepository>();
builder.Services.AddScoped<IRequestwisefileRepository, RequestwisefileRepository>();


builder.Services.AddScoped<IAllRequestDataRepository , AllRequestDataRepository>();
builder.Services.AddScoped<ISendEmailRepository, SendEmailRepository>();
builder.Services.AddScoped<IRequestPopUpActionsRepository , RequestPopUpActionsRepository>();
builder.Services.AddScoped<IDocumentsRepository , DocumentsRepository>();
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
    name: "Admin",
    pattern: "{area=AdminArea}/{controller=Home}/{action=AdminLogin}/{id?}");

app.MapControllerRoute(
    name: "Patient",
    pattern: "{area=PatientArea}/{controller=Home}/{action=Index}/{id?}");
app.Run();












