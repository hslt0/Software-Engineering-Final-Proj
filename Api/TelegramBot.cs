using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api;

public class TelegramBot(ITelegramBotClient client, ILogger<TelegramBot> logger)
{
    public async Task Update(Update update, CancellationToken ct = default)
    {
        if (update.Message is not { } message) return;
        
        if (update.Message.Text is not {} messageText) return;
        
        var chatId = message.Chat.Id;
        
        logger.LogInformation("Received a '{MessageText}' message from chat with ID '{ChatId}'", messageText, chatId);
        
        await client.SendMessage(
            chatId,
            messageText,
            cancellationToken: ct);
    }
}