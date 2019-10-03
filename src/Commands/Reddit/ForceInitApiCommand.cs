using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal class ForceInitRedditApiCommand : Command
    {
        internal const string Trigger = "forceinit";
        internal static readonly string[] Triggers = {RedditBaseCommand.BaseTrigger, Trigger};

        protected override bool IsTriggeredByMessage(MessageContext context) =>
            context.SatisfiesTriggers(Triggers, canTakeArguments: false);

        protected override async Task _Invoke(MessageContext context)
        {
            var initialMessage = "Attempting to establish Reddit API, ";
            string[] splitMessage = Regex.Split(context.MessageString, @"[\s+]");
            if(!(splitMessage.Length > 2 && int.TryParse(splitMessage[2], out var timeout)))
            {
                timeout = Reddit.DefaultTimeout;
            }

            initialMessage += $"timeout={timeout}ms...";
            await context.Reply(initialMessage).ConfigureAwait(false);
            await context.Reply(await Reddit.EstablishApi(timeout)
.ConfigureAwait(false) ? "Reddit API successfully established!"
                : "Error: could not establish Reddit API.").ConfigureAwait(false);
        }
    }
}