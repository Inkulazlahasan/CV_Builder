using CV_Builder.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==================================================
// 1. ADD DEFAULT MVC SERVICES (Required for any website)
// ==================================================
builder.Services.AddControllersWithViews();

// ==================================================
// 2. ADD DATABASE CONTEXT (Connects to SQL Server)
// ==================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==================================================
// 3. ADD SESSION SERVICES (For Admin Login to work)
// ==================================================
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ==================================================
// 4. HTTP REQUEST PIPELINE (Merged from default template)
// ==================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ==================================================
// 5. IMPORTANT: Enable Session Middleware (Added this!)
// ==================================================
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();