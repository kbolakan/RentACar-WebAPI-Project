using Microsoft.EntityFrameworkCore; // Bu kütüphaneyi en üste eklemeyi unutma
using Rentt.Data; // Kendi projendeki Data klasörünün yolunu belirtir
using Rentt.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// --- EKLENECEK KISIM BURASI ---
// Veri tabaný servisimizi (AppDbContext) sisteme kaydediyoruz ve baðlantý dizesini veriyoruz.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Sisteme diyoruz ki: Biri senden IRentalService isterse, ona RentalService sýnýfýný ver!
builder.Services.AddScoped<IRentalService, RentalService>();

// YENÝ EKLENEN KABLO: Biri senden IMaintenanceService isterse, MaintenanceService'i ver!
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
// ------------------------------

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Sonsuz döngüye girmeyi engeller, referans döngülerini yok sayar!
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
