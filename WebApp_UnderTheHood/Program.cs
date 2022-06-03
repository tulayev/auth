var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication("custom_cookie_auth")
    .AddCookie("custom_cookie_auth", 
    options =>
    {
        options.Cookie.Name = "custom_cookie_auth";
        options.LoginPath = "/account/login";
        options.AccessDeniedPath = "/account/accessdenied";
    });

builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy("admin_policy", policy => policy.RequireClaim("Admin"));
        options.AddPolicy("hr_policy", policy => policy.RequireClaim("Department", "HR"));
        options.AddPolicy("hr_manager_policy", policy => policy.RequireClaim("Department", "HR").RequireClaim("HRManager", "true"));
    });

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
