
using CinemaManagement.Data;
using CinemaManagement.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));

builder.Services.AddScoped<FilmeService>();
builder.Services.AddScoped<SalaService>();
builder.Services.AddScoped<SessaoService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<IngressoService>();
builder.Services.AddScoped<AssentoService>();
builder.Services.AddScoped<ReservaService>();
builder.Services.AddScoped<CompraService>();

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
