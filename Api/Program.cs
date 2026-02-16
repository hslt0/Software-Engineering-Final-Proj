using Api;
using Scalar.AspNetCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddHttpClient("TelegramBotClient")
    .AddTypedClient<ITelegramBotClient>((httpClient, _) =>
    {
        var token = builder.Configuration["TelegramBotToken"];
        return string.IsNullOrEmpty(token)
            ? throw new InvalidOperationException("Telegram Bot Token not set")
            : new TelegramBotClient(token, httpClient);
    });

builder.Services.AddHealthChecks();

builder.Services.AddScoped<TelegramBot>();

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

app.UseHealthChecks("/health");

app.UseHttpsRedirection();

app.MapPost("/", async (Update update, TelegramBot bot, CancellationToken ct) =>
{
    try
    {
        await bot.Update(update, ct);
    }
    catch (Exception e)
    {
        app.Logger.LogError(e, "Error handling update");
    }
});
    
app.Run();