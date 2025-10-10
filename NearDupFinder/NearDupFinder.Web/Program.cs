using NearDupFinder.Dominio.Repositorios;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using NearDupFinder.Aplicacion;
using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Interfaces;
using NearDupFinder.Dominio.Servicios;
using NearDupFinder.Infraestructura.Repositorios;
using NearDupFinder.Web.Auth;
using NearDupFinder.Web.Components;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IDeteccionRepositorio, DeteccionRepositorio>();
builder.Services.AddSingleton<IItemRepositorio, ItemRepositorio>();
builder.Services.AddScoped<GestorClusters>(); // Asegúrate de tener esta interfaz
builder.Services.AddScoped<IServicioEstadosDuplicacion, ServicioEstadosDuplicacion>();

builder.Services.AddSingleton<IRepositorioUsuarios, UsuariosEnMemoriaRepositorio>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<Seguridad>();
builder.Services.AddScoped<LoginService>();


builder.Services.AddSingleton<IAuditoriaRepository, AuditoriaEnMemoriaRepository>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";  
        options.AccessDeniedPath = "/access-denied";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
    });


builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
 
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();