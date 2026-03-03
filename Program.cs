using FFMpegCore;
using Microsoft.AspNetCore.Http.Features;
using VideoUploadServer.Middlewares;
using VideoUploadServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar caminho do FFmpeg
GlobalFFOptions.Configure(options =>
{
    options.BinaryFolder = @"C:\ffmpeg\bin";
});


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configura o limite do Kestrel (servidor)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = long.MaxValue;

});

// Configura o limite do Form
builder.Services.Configure<FormOptions>(options =>
{

    options.MultipartBodyLengthLimit = long.MaxValue;
});

builder.Services.AddControllers();

// Service
builder.Services.AddScoped<UploadService>();
builder.Services.AddScoped<StorageService>();


var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.MapControllers();

app.Run();




