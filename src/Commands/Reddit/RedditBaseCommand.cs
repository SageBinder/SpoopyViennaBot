using System.Threading.Tasks;
using Reddit;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal class RedditBaseCommand : Command
    {
        internal const string BaseTrigger = "!reddit";

        protected override bool IsTriggeredByMessage(MessageContext context) =>
            context.SatisfiesTriggers(new[] {BaseTrigger}, canTakeArguments: false);

        protected override async Task _Invoke(MessageContext context) =>
            await context.Reply("Error: no reddit command specified. (TODO: Write a better error message for this lol)");

        public static async Task<RedditAPI> EstablishApiIfNecessaryAndGet(MessageContext context,
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