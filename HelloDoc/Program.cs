using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using HelloDoc;

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
builder.Services.AddScoped<IRequestStatusLogRepository, RequestStatusLogRepository>();
builder.Services.AddScoped<IRequestwisefileRepository, RequestwisefileRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IPhysicianRepository, PhysicianRepository>();
builder.Services.AddScoped<IRoleMenuRepository, RoleMenuRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();


builder.Services.AddScoped<IAllRequestDataRepository, AllRequestDataRepository>();
builder.Services.AddScoped<ISendEmailRepository, SendEmailRepository>();
builder.Services.AddScoped<IRequestPopUpActionsRepository, RequestPopUpActionsRepository>();
builder.Services.AddScoped<IDocumentsRepository, DocumentsRepository>();
builder.Services.AddScoped<ISessionUtilsRepository, SessionUtilsRepository>();
builder.Services.AddScoped<IAuthorizatoinRepository, AuthorizationRepository>();
builder.Services.AddScoped<IJwtRepository, JwtRepository>();
builder.Services.AddScoped<IPatientFormsRepository, PatientFormsRepository>();
builder.Services.AddScoped<IPaginationRepository, PaginationRepository>();
builder.Services.AddScoped<IProviderMenuRepository, ProviderMenuRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCekx0Q3xbf1x0ZFFMYF5bQHRPMyBoS35RckVgW3hedHFQRmBdUkR0");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();


app.MapControllerRoute(
    name: "Admin",
    pattern: "{area=AdminArea}/{controller=Home}/{action=AdminTabsLayout}/{id?}");

app.MapControllerRoute(
    name: "Patient",
    pattern: "{area=PatientArea}/{controller=Home}/{action=Index}/{id?}");
app.Run();












