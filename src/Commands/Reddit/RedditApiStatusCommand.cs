using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    public class RedditApiStatusCommand : Command
    {
        private const string Trigger = "status";
        
        public override bool IsTriggeredByMessage(CommandContext context)
        {
            return MessageMatchesTriggerList(context.Message, new[] {Reddit.BaseTriggerString, Trigger},
                canTakeArguments: false);
        }

        public override async Task Invoke(CommandContext context)
        {
            await context.Reply("Checking reddit API connection status...");
            
            bool apiIsEstablished = await Reddit.ApiIsEstablished();
            if(apiIsEstablished)
            {
                await context.Reply("The connection to the Reddit API appears to be working");
            }
            else
            {
                await context.Reply("Error: It appears the connection to the Reddit API is not working");
            }
        }
    }
}