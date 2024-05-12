using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NodaTime;
using NodaTime.Api;
using NodaTime.Serialization.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//To get correct swagger types, format and examples.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NodaTime.Api", Version = "v1" });
    c.MapType<LocalDateTime>(() => new OpenApiSchema { Type = "string", Format = "date-time", Example = new OpenApiString("2024-05-11T21:57:56.9420212") });
    c.MapType<LocalDate>(() => new OpenApiSchema { Type = "string", Format = "date", Example = new OpenApiString("2024-05-11") });
    c.MapType<OffsetDateTime>(() => new OpenApiSchema { Type = "string", Format = "date-time", Example = new OpenApiString("2024-05-11T21:47:15.0426167+02:00") });
    c.MapType<ZonedDateTime>(() => new OpenApiSchema { Type = "string", Format = "date-time", Example = new OpenApiString("2024-05-11T19:50:27.8635446Z UTC") });
    c.MapType<Instant>(() => new OpenApiSchema { Type = "string", Format = "date-time" });
});
builder.Services.AddSingleton<IClock>(SystemClock.Instance);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGroup("time").MapTimeApis().WithDisplayName("Times");
app.MapGroup("convert").MapConvertApis().WithDisplayName("Times");
app.Run();
