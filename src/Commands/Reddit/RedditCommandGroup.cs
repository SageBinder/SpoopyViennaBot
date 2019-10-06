using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    public class RedditCommandGroup : CommandGroup
    {
        public RedditCommandGroup()
        {
            var context = new RedditContext();
            var getCommand = new GetPostCommand(context);

            Commands.Add(new RedditBaseCommand(context));
            Commands.Add(getCommand);
            Commands.Add(new GetAskRedditQuestionCommand(context));
            Commands.Add(new ForceInitRedditApiCommand(context));
            Commands.Add(new RedditApiStatusCommand(context));
            Commands.Add(new NextCommand(context));
            Commands.Add(new PreviousCommand(context));
            Commands.Add(new FeedCommand(context));
            
            context.GetCommand = getCommand;
        }
    }
}