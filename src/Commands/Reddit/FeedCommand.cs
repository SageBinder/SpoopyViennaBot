using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal class FeedCommand : RedditBaseCommand
    {
        internal const string Trigger = "feed";
        internal readonly string[] Triggers = {BaseTrigger, Trigger};

        internal FeedCommand(RedditContext commandContext) : base(commandContext)
        {
        }

        protected override bool IsTriggeredByMessage(MessageContext context) => context.SatisfiesTriggers(Triggers);

        protected override async Task _Invoke(MessageContext context)
        {
            var feedArg = string.Join("/", context.GetSequentialArgs(Triggers.Length));
            if(feedArg.Length == 0)
            {
                await context.Reply($"Current feed: {CommandContext.CurrentFeed}")
                    .ConfigureAwait(false);
                return;
            }

            var feedArgMatch = Regex.Match(feedArg, @"([a-zA-Z0-9_]+)(?:[\s|/\\]+([a-zA-Z]+))?");
            var subredditName = feedArgMatch.Groups[1].ToString();
            var feedTypeString = feedArgMatch.Groups[2].ToString().Trim().Length > 0 ? feedArgMatch.Groups[2].ToString() : "hot";
            RedditFeed.FeedType feedType;
            
            switch(char.ToLower(feedTypeString[0]))
            {
                case 'h':
                    feedType = RedditFeed.FeedType.Hot;
                    break;
                case 'n':
                    feedType = RedditFeed.FeedType.New;
                    break;
                case 'r':
                    feedType = RedditFeed.FeedType.Rising;
                    break;
                case 'c':
                    feedType = RedditFeed.FeedType.Controversial;
                    break;
                case 't':
                    feedType = RedditFeed.FeedType.Top;
                    break;
                default:
                    feedType = RedditFeed.FeedType.Hot;
                    break;
            }
            
            CommandContext.SetCurrentFeed(subredditName, feedType);
            await context.Reply($"Set Reddit feed to {CommandContext.CurrentFeed}")
                .ConfigureAwait(false);
        }
    }
}