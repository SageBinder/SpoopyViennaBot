using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    public class RedditCommandGroup : CommandGroup
    {
        public RedditCommandGroup()
        {
            Commands.Add(new RedditBaseCommand());
            Commands.Add(new GetPostCommand());
            Commands.Add(new GetAskRedditQuestionCommand());
            Commands.Add(new ForceInitRedditApiCommand());
            Commands.Add(new RedditApiStatusCommand());
        }
    }
}