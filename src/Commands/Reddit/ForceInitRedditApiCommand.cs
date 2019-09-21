using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    public class ForceInitRedditApiCommand : Command
    {
        private const string Trigger = "forceinit";
        
        public override bool IsTriggeredByMessage(CommandContext context)
        {
            return MessageMatchesTriggerList(context.Message,
                new[] {Reddit.BaseTriggerString, Trigger});
        }

        public override async Task Invoke(CommandContext context)
        {
            var initialMessage = "Attempting to establish Reddit API, ";
            string[] splitMessage = Regex.Split(context.Message, @"[\s+]");
            if(!(splitMessage.Length > 2 && int.TryParse(splitMessage[2], out var timeout)))
            {
                timeout = Reddit.defaultTimeout;
            }

            initialMessage += $"timeout={timeout}ms...";
            await context.Reply(initialMessage);
            await context.Reply(await Reddit.EstablishApi(timeout)
                ? "Reddit API successfully established!"
                : "Error: could not establish Reddit API.");
        }
    }
}