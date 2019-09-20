using System.Linq;
using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;
using Reddit.Inputs;

namespace SpoopyViennaBot.Commands.Reddit
{
    public class GetAskRedditQuestionCommand : Command
    {
        private const string Trigger = "question";

        public override bool IsTriggeredByMessage(CommandContext context)
        {
            return MessageMatchesTriggerList(context.Message,
                new[] {Reddit.BaseTriggerString, Trigger});
        }

        public override async Task Invoke(CommandContext context)
        {
            var reddit = Reddit.GetApi();
            if(reddit == null)
            {
                await context.Reply("Error: A connection to the Reddit API has not yet established. Now attempting to establish...");
                reddit = await Reddit.EstablishApiAndGet();
            }
            
            var askReddit = reddit.Subreddit("AskReddit").About();
            var latestPost = askReddit.Posts.GetNew(new CategorizedSrListingInput(count: 1)).First();
            await context.Reply(
                string.Format("{0} ({1} upvote{2}, {3} downvote{4})",
                    latestPost.Title,
                    latestPost.UpVotes,
                    latestPost.UpVotes != 1 ? "s" : "",
                    latestPost.DownVotes,
                    latestPost.DownVotes != 1 ? "s" : ""));
        }
    }
}