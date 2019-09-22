using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    public class GetAskRedditQuestionCommand : Command
    {
        private const string Trigger = "question";
        private static readonly string[] Triggers = {RedditBaseCommand.BaseTrigger, Trigger};

        public override bool IsTriggeredByMessage(CommandContext context) =>
            context.SatisfiesTriggers(Triggers);

        public override async Task Invoke(CommandContext context)
        {
            await GetPostCommand._invoke(context, new[] {"AskReddit"});
        }
    }
}