using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    public class InitRedditApiCommand : Command
    {
        private const string Trigger = "init";
        
        public override bool IsTriggeredByMessage(CommandContext context)
        {
            return MessageMatchesTriggerList(context.Message,
                new[] {Reddit.BaseTriggerString, Trigger});
        }

        public override async Task Invoke(CommandContext context)
        {
            string initialMessage = "Attempting to establish Reddit API, ";
            string[] splitMessage = Regex.Split(context.Message, @"[\s+]");
            if(splitMessage.Length > 1 && int.TryParse(splitMessage[1], out var timeout))
            {
                initialMessage += "timeout=" + timeout + "ms...";
                await context.Reply(initialMessage);
                await context.Reply(await Reddit.EstablishApi(timeout)
                    ? "Reddit API successfully established!"
                    : "Error: could not establish Reddit API.");
            }
            else
            {
                initialMessage += "timeout=30000ms...";
                await context.Reply(initialMessage);
                await context.Reply(await Reddit.EstablishApi()
                    ? "Reddit API successfully established!"
                    : "Error: could not establish Reddit API.");
            }
        }
    }
}