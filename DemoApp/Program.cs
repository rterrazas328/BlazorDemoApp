using DemoApp.Components;
using DemoApp.Data;
using DemoApp.Services;
using DemoApp.Services.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<DemoAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DemoAppContext") ?? throw new InvalidOperationException("Connection string 'DemoAppContext' not found."))    
);

builder.Services.AddDbContextFactory<DemoAppLoginContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DemoAppContext") ?? throw new InvalidOperationException("Connection string 'DemoAppLoginContext' not found."))
);

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();

builder.Services.AddScoped<AuthenticationStateProvider, MainLoginAuthenticationStateProvider>();
//builder.Services.AddScoped<MainLoginAuthenticationStateProvider>();
//builder.Services.AddScoped<AuthenticationStateProvider> (sp => sp.GetRequiredService<MainLoginAuthenticationStateProvider>());

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie( options =>
{
    options.LoginPath = "/login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddAuthorization();


builder.Services.AddControllers();

builder.Services.AddScoped<IUserService, UserService>();


/*builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    }
);*/


//Loggin setup
builder.Logging.ClearProviders();
builder.Logging.AddEventLog(eventLogSettings =>
{
    eventLogSettings.SourceName = "DemoApp";
});

builder.Logging.AddConsole(); //For debugging only
builder.Logging.AddDebug(); //For debugging only

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.MapControllers();

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
