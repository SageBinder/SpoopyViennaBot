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
                await context.Reply($"Current feed: {(CommandContext.CurrentFeed != null ? CommandContext.CurrentFeed.ToString() : "(no feed)")}")
                    .ConfigureAwait(false);
                return;
            }

            var feedArgMatch = Regex.Match(feedArg, @"([a-zA-Z0-9_]+)(?:[\s|/\\]+([a-zA-Z]+))?");
            var subredditName = feedArgMatch.Groups[1].ToString();
            var feedTypeString = feedArgMatch.Groups[2].ToString().Trim().Length > 0 ? feedArgMatch.Groups[2].ToString() : "hot";
            var feedType = RedditFeed.GetFeedTypeFromChar(feedTypeString[0]);

            CommandContext.SetCurrentFeed(subredditName, feedType);
            await context.Reply($"Set Reddit feed to {CommandContext.CurrentFeed}")
                .ConfigureAwait(false);
        }
    }
}