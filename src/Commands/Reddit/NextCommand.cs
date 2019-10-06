using System.Threading.Tasks;
using Reddit;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal class NextCommand : RedditBaseCommand
    {
        internal const string Trigger = "next";
        internal readonly string[] Triggers = {BaseTrigger, Trigger};
        
        internal NextCommand(RedditContext commandContext) : base(commandContext)
        {
        }

        protected override bool IsTriggeredByMessage(MessageContext context) => context.SatisfiesTriggers(Triggers);

        protected override async Task _Invoke(MessageContext context)
        {
            if(CommandContext.CurrentFeed == null)
            {
                await context.Reply(
                    "Error: no Reddit feed currently specified. Run `!reddit feed [subreddit_name]/[sorting]` to specify a feed.")
                    .ConfigureAwait(false);
                return;
            }
            
            RedditAPI reddit;
            if((reddit = await EstablishApiIfNecessaryAndGet(context).ConfigureAwait(false)) == null) return;

            var post = CommandContext.CurrentFeed.Next(reddit);
            await context.Reply(
                $"**(+{post.UpVotes}) r/{post.Subreddit}/{CommandContext.CurrentFeed.Properties.Type:G}:** *\"{post.Title}\"*\n" +
                $"{post.Listing.URL}")
                .ConfigureAwait(false);
        }
    }
}