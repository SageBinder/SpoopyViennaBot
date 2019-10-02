using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal class GetAskRedditQuestionCommand : Command
    {
        internal const string Trigger = "question";
        internal static readonly string[] Triggers = {RedditBaseCommand.BaseTrigger, Trigger};

        protected override bool IsTriggeredByMessage(MessageContext context) =>
            context.SatisfiesTriggers(Triggers);

        protected override Task _Invoke(MessageContext context)
        {
            return GetPostCommand._invoke(context, new[] { "AskReddit" });
        }
    }
}