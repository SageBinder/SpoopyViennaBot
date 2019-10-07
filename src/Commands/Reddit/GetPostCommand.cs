using System;
using System.Threading.Tasks;
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

        protected override async Task _Invoke(MessageContext context)
        {
            var args = context.GetSequentialArgs(Triggers.Length);
            if(args.Length == 0)
            {
                await context.Reply($"Error: no arguments provided. Usage: `{Triggers[0]} {Triggers[1]} <subreddit> [feed_type]`").ConfigureAwait(false);
                return;
            }

            await ReplyWithPosts(context, args);
        }

        internal async Task ReplyWithPosts(MessageContext context, string[] args)
        {
            var subredditName = args[0];
            var feedTypeString = args.Length > 1 ? args[1] : "hot";
            int count = int.TryParse(args.Length > 2 ? args[2] : "1", out count) ? count : 1;
            if(count > MaxPostsPerInvoke) count = MaxPostsPerInvoke;

            var oldFeed = CommandContext.CurrentFeed;
            CommandContext.SetCurrentFeed(subredditName, RedditFeed.GetFeedTypeFromChar(feedTypeString[0]));

            await CommandContext.NextCommand.ReplyWithPosts(context, new[] {count.ToString()});

            if(oldFeed == null)
            {
                CommandContext.ResetCurrentFeed();
            }
            else
            {
                CommandContext.SetCurrentFeed(oldFeed.Properties);
            }
        }
    }
}