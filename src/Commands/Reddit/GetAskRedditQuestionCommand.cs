using System.Linq;
using System.Threading.Tasks;
using Reddit;
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
            RedditAPI reddit;
            if(!await Reddit.ApiIsEstablished())
            {
                await context.Reply("It appears the connection to the Reddit API has failed... Attempting to reestablish");
                reddit = await Reddit.EstablishApiAndGet();
            }
            else
            {
                reddit = Reddit.GetApi();
            }

            if(reddit == null)
            {
                await context.Reply("Error: a connection to the Reddit API could not be established.");
                return;
            }
            
            var askReddit = reddit.Subreddit("AskReddit").About();
            var latestPost = askReddit.Posts.GetNew(new CategorizedSrListingInput(count: 1)).First();
            await context.Reply(
                $"r/AskReddit: *\"{latestPost.Title}\"*\n" +
                $"({latestPost.UpVotes} upvote{(latestPost.UpVotes != 1 ? "s" : "")}, " +
                $"{latestPost.DownVotes} downvote{(latestPost.DownVotes != 1 ? "s" : "")})");
        }
    }
}