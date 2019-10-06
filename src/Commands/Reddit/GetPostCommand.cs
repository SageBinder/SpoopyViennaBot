using System;
using System.Linq;
using System.Threading.Tasks;
using Reddit;
using Reddit.Controllers;
using Reddit.Inputs;
using Reddit.Inputs.Listings;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    // TODO: Update this to use the feed-based reddit commands
    internal class GetPostCommand : RedditBaseCommand
    {
        internal const string Trigger = "get";
        internal static readonly string[] Triggers = {BaseTrigger, Trigger};

        internal GetPostCommand(RedditContext commandContext) : base(commandContext)
        {
        }

        protected override bool IsTriggeredByMessage(MessageContext context) => context.SatisfiesTriggers(Triggers);

        protected override Task _Invoke(MessageContext context)
        {
            var commandArgs = context.GetSequentialArgs(Triggers.Length);
            return _invoke(context, commandArgs);
        }

        // Fuck, this is retarded. I don't know how it happened. Whatever, I don't care right now
        internal async Task _invoke(MessageContext context, string[] args)
        {
            if(args.Length == 0)
            {
                await context.Reply($"Error: no arguments provided. Usage: `{Triggers[0]} {Triggers[1]} <subreddit> [feed_type]`").ConfigureAwait(false);
                return;
            }

            var subredditName = args[0];

            var count = 1;
            if(args.Length >= 2 && int.TryParse(args[1], out count))
            {
                if(count > 5) count = 5;
                await ReplyWithPosts(context, subredditName, "hot", count);
            }
            else
            {
                var feedType = "hot";
                if(args.Length >= 2)
                {
                    feedType = args[1];
                }

                if(args.Length >= 3)
                {
                    count = int.TryParse(args[2], out count) ? count : 1;
                }

                if(count > 5) count = 5;

                await ReplyWithPosts(context, subredditName, feedType, count);
            }
        }

        private async Task ReplyWithPosts(MessageContext context, string subredditName, string feedType, int count)
        {
            RedditAPI reddit;
            if((reddit = await EstablishApiIfNecessaryAndGet(context).ConfigureAwait(false)) == null) return;
            Subreddit subreddit;
            try
            {
                subreddit = reddit.Subreddit(subredditName).About();
            }
            catch(Exception)
            {
                await context.Reply(
                    $"Error: couldn't access the subreddit *r/{subredditName}*. Are you sure it exists?")
                    .ConfigureAwait(false);
                return;
            }

            for(var i = 0; i < count; i++)
            {
                Post post;
                if(char.ToLower(feedType[0]) == 'b')
                    post = subreddit.Posts.GetBest(new CategorizedSrListingInput()).SkipWhile(p => CommandContext.SeenPosts.Contains(p.Id)).First();
                else if(char.ToLower(feedType[0]) == 'h')
                    post = subreddit.Posts.GetHot(new ListingsHotInput()).SkipWhile(p => CommandContext.SeenPosts.Contains(p.Id)).First();
                else if(char.ToLower(feedType[0]) == 'n')
                    post = subreddit.Posts.GetNew(new CategorizedSrListingInput()).SkipWhile(p => CommandContext.SeenPosts.Contains(p.Id)).First();
                else if(char.ToLower(feedType[0]) == 'r')
                    post = subreddit.Posts.GetRising(new CategorizedSrListingInput()).SkipWhile(p => CommandContext.SeenPosts.Contains(p.Id)).First();
                else if(char.ToLower(feedType[0]) == 'c')
                    post = subreddit.Posts.GetControversial(new TimedCatSrListingInput()).SkipWhile(p => CommandContext.SeenPosts.Contains(p.Id)).First();
                else if(char.ToLower(feedType[0]) == 't')
                    post = subreddit.Posts.GetTop().SkipWhile(p => CommandContext.SeenPosts.Contains(p.Id)).First();
                else
                    post = subreddit.Posts.GetHot(new ListingsHotInput()).SkipWhile(p => CommandContext.SeenPosts.Contains(p.Id)).First();

                CommandContext.SeenPosts.Add(post.Id);
            
                await context.Reply(
                        $"**(+{post.UpVotes}) r/{post.Subreddit}/{feedType}:** *\"{post.Title}\"*\n" +
                        $"{post.Listing.URL}")
                    .ConfigureAwait(false);
            }
        }
    }
}