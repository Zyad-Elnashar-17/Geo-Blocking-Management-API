using BlockedCountries.BackgroundServices;
using BlockedCountries.Interfaces;
using BlockedCountries.Repositories;
using BlockedCountries.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpClient<IGeolocationService, GeolocationService>(client =>
{
    client.BaseAddress = new Uri("https://ipapi.co/");
    client.DefaultRequestHeaders.Add("User-Agent", "GeoBlocker-App");
});
// Add services to the container.

builder.Services.AddSingleton<IBlockedCountryRepository, BlockedCountryRepository>();
builder.Services.AddSingleton<ITemporaryBlockRepository, TemporaryBlockRepository>();
builder.Services.AddSingleton<IBlockAttemptLogRepository, BlockAttemptLogRepository>();


builder.Services.AddHostedService<TemporalBlockCleanupService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
