using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace SpoopyViennaBot.Utils.CommandsMeta
{
    public class MessageContext
    {
        public readonly DiscordClient BotClient;
        public readonly MessageCreateEventArgs MessageEvent;
        
        public string MessageString => MessageEvent.Message.Content;
        public DiscordUser Author => MessageEvent.Author;

        public MessageContext(DiscordClient botClient, MessageCreateEventArgs messageEvent)
        {
            BotClient = botClient;
            MessageEvent = messageEvent;
        }
        
        public async Task<DiscordMessage> Reply(string message) => await MessageEvent.Channel.SendMessageAsync(message).ConfigureAwait(false);

        public bool SatisfiesTriggers(string[] triggers,
            string delimRegex = @"\s+",
            bool caseSensitive = false,
            bool matchWithRegex = false,
            bool canTakeArguments = true)
        {
            var splitMessage = Regex.Split(caseSensitive ? MessageString : MessageString.ToLower(), delimRegex);
            if(splitMessage.Length < triggers.Length || !canTakeArguments && splitMessage.Length > triggers.Length)
            {
                return false;
            }

            if(matchWithRegex)
            {
                return !triggers.Where((word, i) => !Regex.Match(caseSensitive ? word : word.ToLower(), splitMessage[i]).Success).Any();
            }

            return !triggers.Where((word, i) => !(caseSensitive ? word : word.ToLower()).Equals(splitMessage[i])).Any();
        }
        
        public string[] GetSequentialArgs(int numTriggers = 0, string delimRegex = @"\s+")
        {
            var splitString = Regex.Split(MessageString, delimRegex);
            return splitString.Skip(numTriggers).ToArray();
        }
    }
}