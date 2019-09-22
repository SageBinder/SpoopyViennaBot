using System.Threading.Tasks;
using Reddit;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    public class RedditBaseCommand : Command
    {
        internal const string BaseTrigger = "!reddit";
        
        public override bool IsTriggeredByMessage(CommandContext context) =>
            context.SatisfiesTriggers(new[] {BaseTrigger}, canTakeArguments: false);

        public override async Task Invoke(CommandContext context) =>
            await context.Reply("Error: no reddit command specified. (TODO: Write a better error message for this lol)");

        public static async Task<RedditAPI> EstablishApiIfNecessaryAndGet(CommandContext context,
            int timeout = Reddit.DefaultTimeout,
            bool displayMessages = true)
        {
            if(!displayMessages)
            {
                return await Reddit.EstablishApiIfNecessaryAndGet(timeout);
            }
            
            RedditAPI reddit;
            if(!Reddit.ApiIsEstablished())
            {
                await context.Reply("It appears the connection to the Reddit API has failed... Attempting to reestablish");
                reddit = await Reddit.EstablishApiAndGet(timeout);
            }
            else
            {
                reddit = Reddit.Api;
            }

            if(reddit == null)
            {
                await context.Reply("Error: a connection to the Reddit API could not be established.");
            }

            return reddit;
        }
    }
}