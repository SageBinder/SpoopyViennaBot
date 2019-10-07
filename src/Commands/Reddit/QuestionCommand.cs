using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal class QuestionCommand : RedditBaseCommand
    {
        internal const string Trigger = "question";
        internal static readonly string[] Triggers = {BaseTrigger, Trigger};

        protected override bool IsTriggeredByMessage(MessageContext context) => context.SatisfiesTriggers(Triggers);

        protected override async Task _Invoke(MessageContext context)
        {
            var args = context.GetSequentialArgs(Triggers.Length);
            var feedTypeString = "hot";

            if(args.Length > 0 && int.TryParse(args[0], out var count))
            {
                await CommandContext.GetCommand.ReplyWithPosts(context,
                    new[] {"AskReddit", feedTypeString, $"{count}"});
                return;
            }

            feedTypeString = args.Length > 0 ? args[0] : "hot";
            var numQuestionsString = args.Length > 1 ? args[1] : "1";
            await CommandContext.GetCommand.ReplyWithPosts(context,
                new[] {"AskReddit", feedTypeString, numQuestionsString});
        }

        internal QuestionCommand(RedditContext commandContext) : base(commandContext)
        {
        }
    }
}