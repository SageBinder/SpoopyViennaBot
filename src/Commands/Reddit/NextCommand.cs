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

            await ReplyWithPosts(context, context.GetSequentialArgs(Triggers.Length));
        }

        internal async Task ReplyWithPosts(MessageContext context, string[] args)
        {
            var countString = args.Length > 0 ? args[0] : "1";
            int count = int.TryParse(countString, out count) ? count : 1;
            if(count > MaxPostsPerInvoke) count = MaxPostsPerInvoke;
            
            RedditAPI reddit;
            if((reddit = await EstablishApiIfNecessaryAndGet(context).ConfigureAwait(false)) == null) return;

            for(var i = 0; i < count; i++)
            {
                var post = CommandContext.CurrentFeed.Next(reddit);
                await context.Reply(
                        $"**(+{post.UpVotes}) r/{post.Subreddit}/{CommandContext.CurrentFeed.Properties.Type:G}:** *\"{post.Title}\"*\n" + 
                        $"{post.Listing.URL}")
                    .ConfigureAwait(false);
            }
        }
    }
}