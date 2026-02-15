using Scalar.AspNetCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(builder.Configuration["TelegramBotToken"]!));

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

app.MapPost("/", async (Update update, ITelegramBotClient client) =>
{
    if (update.Message == null) return Results.BadRequest();
    
    var chatId = update.Message.Chat.Id;
    await client.SendMessage(chatId, "Hello");

    return Results.Ok();
});
    
app.Run();