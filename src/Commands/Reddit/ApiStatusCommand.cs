using System.Threading.Tasks;
using DSharpPlus.Entities;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal class RedditApiStatusCommand : Command
    {
        internal const string Trigger = "status";
        internal static readonly string[] Triggers = {RedditBaseCommand.BaseTrigger, Trigger};

        protected override bool IsTriggeredByMessage(MessageContext context) =>
            context.SatisfiesTriggers(Triggers, canTakeArguments: false);

        protected override async Task _Invoke(MessageContext context)
        {
            DiscordMessage checkingMessage = await context.Reply("Checking reddit API connection status...").ConfigureAwait(false);
            
            bool apiIsEstablished = Reddit.ApiIsEstablished();
            if(apiIsEstablished)
            {
                await context.Reply(":white_check_mark: The connection to the Reddit API appears to be working").ConfigureAwait(false);
            }
            else
            {
                await context.Reply(":x: Error: It appears the connection to the Reddit API is not working").ConfigureAwait(false);
            }

            await checkingMessage.DeleteAsync().ConfigureAwait(false);
        }
    }
}