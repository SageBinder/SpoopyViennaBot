using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal class GetAskRedditQuestionCommand : RedditBaseCommand
    {
        internal const string Trigger = "question";
        internal static readonly string[] Triggers = {BaseTrigger, Trigger};

        protected override bool IsTriggeredByMessage(MessageContext context) => context.SatisfiesTriggers(Triggers);

        protected override Task _Invoke(MessageContext context)
        {
            return CommandContext.GetCommand._invoke(context, new[] {"AskReddit", "hot"});
        }

        internal GetAskRedditQuestionCommand(RedditContext commandContext) : base(commandContext)
        {
        }
    }
}