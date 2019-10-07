using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    public class RedditCommandGroup : CommandGroup
    {
        public RedditCommandGroup()
        {
            var context = new RedditContext();
            var getCommand = new GetPostCommand(context);
            var nextCommand = new NextCommand(context);
            var previousCommand = new PreviousCommand(context);

            Commands.Add(new RedditBaseCommand(context));
            Commands.Add(getCommand);
            Commands.Add(new QuestionCommand(context));
            Commands.Add(new ForceInitRedditApiCommand(context));
            Commands.Add(new RedditApiStatusCommand(context));
            Commands.Add(nextCommand);
            Commands.Add(previousCommand);
            Commands.Add(new FeedCommand(context));
            
            context.GetCommand = getCommand;
            context.NextCommand = nextCommand;
            context.PreviousCommand = previousCommand;
        }
    }
}