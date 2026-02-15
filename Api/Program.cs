using Scalar.AspNetCore;
using Telegram.Bot.Types;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

// Add services to the container.
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.MaxDepth = 1024;
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/health", () => "Health OK");

app.MapPost("/", (Update update, ILogger<Program> logger) => 
{
    logger.LogInformation("Отримано текст: {Text}", update.Message?.Text ?? "No text");
    return Results.Ok("Bot is working");
});
    
app.Run();