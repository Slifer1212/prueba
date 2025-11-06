using Application.IServices;
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContext;
using Persistence.Repositories;
using Mapster;
using MapsterMapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// 1. CONFIGURAR CONTROLLERS
// ========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "Task Management API", 
        Version = "v1",
        Description = "API REST para gestión de tareas - Prueba Técnica Backend Jr"
    });
});

// ========================================
// 2. CONFIGURAR DATABASE CONTEXT ✅
// ========================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Persistence")));

// ========================================
// 3. CONFIGURAR MAPSTER ✅
// ========================================
// Registrar la configuración de Mapster
var config = new TypeAdapterConfig();
// Escanear el assembly de Application para encontrar configuraciones de mapeo
config.Scan(Assembly.GetAssembly(typeof(IUserService))!);

// Registrar Mapster en el contenedor de DI
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, Mapper>();

// ========================================
// 4. CONFIGURAR REPOSITORIOS ✅
// ========================================
builder.Services.AddScoped<IUserRepository, UserRespository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// ========================================
// 5. CONFIGURAR SERVICIOS ✅
// ========================================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();

// ========================================
// 6. CONFIGURAR CORS (Opcional)
// ========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ========================================
// MIDDLEWARE PIPELINE
// ========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();