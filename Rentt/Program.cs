using Microsoft.EntityFrameworkCore;
using Rentt.Business.Abstract;
using Rentt.Business.Services;
using Rentt.Services;
// Eski 'Data' klasörü artýk koca bir DataAccess katmaný oldu!
// ---------------------------------------------

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Veri tabaný servisimizi (AppDbContext) DataAccess katmanýndan alýyoruz.
builder.Services.AddDbContext<Rentt.DataAccess.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ýþ kurallarýmýzý (Business katmanýndaki servisleri) vitrine baðlýyoruz.
builder.Services.AddScoped<IRentalService, Rentt.Business.Services.RentalService>();
builder.Services.AddScoped<IMaintenanceService, Rentt.Business.Services.MaintenanceService>();
builder.Services.AddScoped<IDamageRecordService, DamageRecordService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IAuthService, AuthService>();

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