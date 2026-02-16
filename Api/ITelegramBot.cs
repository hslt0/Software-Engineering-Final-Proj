using Telegram.Bot.Types;

namespace Api;

public interface ITelegramBot
{
    Task Update(Update update, CancellationToken ct);
}