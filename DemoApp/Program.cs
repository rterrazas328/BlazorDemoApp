using DemoApp.Components;
using DemoApp.Data;
using DemoApp.Services;
using DemoApp.Services.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
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

/*builder.Services.AddHttpClient("ServerAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7076");
});*/
builder.Services.AddScoped(sp =>
    {
        var nav = sp.GetRequiredService<NavigationManager>();
        return new HttpClient { BaseAddress = new Uri(nav.BaseUri) };
    });

builder.Services.AddHttpContextAccessor();

//builder.Services.AddSingleton<CustomAuthService>();
//builder.Services.AddScoped<AuthenticationStateProvider, MainLoginAuthenticationStateProvider>();
//builder.Services.AddScoped<MainLoginAuthenticationStateProvider>();
//builder.Services.AddScoped<AuthenticationStateProvider> (sp => sp.GetRequiredService<MainLoginAuthenticationStateProvider>());

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie( options =>
{
    options.Cookie.Name = ".DemoApp.Auth";
    options.LoginPath = "/api/auth/login";
    options.LogoutPath = "/api/auth/logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddAuthorization();


builder.Services.AddControllers();


//Disable Automatic ModelState Handling
/*builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});//*/

builder.Services.AddScoped<IUserService, UserService>();





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



app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers();
app.MapBlazorHub();




app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
