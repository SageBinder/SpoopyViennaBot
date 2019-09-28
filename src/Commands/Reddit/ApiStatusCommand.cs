using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    public class RedditApiStatusCommand : Command
    {
        internal const string Trigger = "status";
        internal static readonly string[] Triggers = {RedditBaseCommand.BaseTrigger, Trigger};
        
        public override bool IsTriggeredByMessage(CommandContext context) =>
            context.SatisfiesTriggers(Triggers, canTakeArguments: false);

        public override async Task Invoke(CommandContext context)
        {
            await context.Reply("Checking reddit API connection status...");
            
            bool apiIsEstablished = Reddit.ApiIsEstablished();
            if(apiIsEstablished)
            {
                await context.Reply(":white_check_mark: The connection to the Reddit API appears to be working");
            }
            else
            {
                await context.Reply(":x: Error: It appears the connection to the Reddit API is not working");
            }
        }
    }
}