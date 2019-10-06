using System.Collections.Generic;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal class RedditContext
    {
        internal GetPostCommand GetCommand;
        internal readonly HashSet<string> SeenPosts = new HashSet<string>();
        
        private readonly Dictionary<RedditFeed.FeedProperties, RedditFeed> _feeds = new Dictionary<RedditFeed.FeedProperties, RedditFeed>();

        internal RedditFeed CurrentFeed { get; private set; }

        internal void SetCurrentFeed(string subredditName, RedditFeed.FeedType type)
        {
            SetCurrentFeed(new RedditFeed.FeedProperties(subredditName, type));
        }

        internal void SetCurrentFeed(RedditFeed.FeedProperties properties)
        {
            if(_feeds.ContainsKey(properties))
            {
                CurrentFeed = _feeds[properties];
            }
            else
            {
                CurrentFeed = new RedditFeed(properties);
                _feeds.Add(properties, CurrentFeed);
            }
        }
    }
}