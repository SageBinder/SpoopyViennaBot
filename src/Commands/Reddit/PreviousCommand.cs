using System.Threading.Tasks;
using Reddit;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal class PreviousCommand : RedditBaseCommand
    {
        internal const string Trigger = "prev";
        internal readonly string[] Triggers = {BaseTrigger, Trigger};

        internal PreviousCommand(RedditContext commandContext) : base(commandContext)
        {
        }

        protected override bool IsTriggeredByMessage(MessageContext context) => context.SatisfiesTriggers(Triggers);

        protected override async Task _Invoke(MessageContext context)
        {
            RedditAPI reddit;
            if((reddit = await EstablishApiIfNecessaryAndGet(context).ConfigureAwait(false)) == null) return;
            
            var post = CommandContext.CurrentFeed.Previous(reddit);
            await context.Reply(
                    $"**(+{post.UpVotes}) r/{post.Subreddit}/{CommandContext.CurrentFeed.Properties.Type:G}:** *\"{post.Title}\"*\n" +
                    $"{post.Listing.URL}")
                .ConfigureAwait(false);
        }
    }
}