using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace SpoopyViennaBot.Utils.CommandsMeta
{
    public class CommandContext
    {
        public readonly DiscordClient BotClient;
        public readonly MessageCreateEventArgs MessageEvent;
        
        public string Message => MessageEvent.Message.Content;
        public DiscordUser Author => MessageEvent.Author;

        public CommandContext(DiscordClient botClient, MessageCreateEventArgs messageEvent)
        {
            this.BotClient = botClient;
            this.MessageEvent = messageEvent;
        }
        
        public async Task<DiscordMessage> Reply(string message)
        {
            return await MessageEvent.Channel.SendMessageAsync(message);
        }
    }
}