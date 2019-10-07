using System.Collections.Generic;
using System.Linq;
using Reddit;
using Post = Reddit.Controllers.Post;
using Subreddit = Reddit.Controllers.Subreddit;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal class RedditFeed
    {
        internal readonly FeedProperties Properties;

        // Stores posts using their Fullname property, which is unique fore each post across Reddit.
        private readonly List<string> _seenPosts = new List<string>();
        internal int PostIndex { get; private set; } = -1;
        internal int FeedCount => _seenPosts.Count;

        internal RedditFeed(FeedProperties properties)
        {
            Properties = properties;
        }

        internal Post Next(RedditAPI reddit, bool skipSeenPosts = true)
        {
            if(++PostIndex < _seenPosts.Count)
            {
                return reddit.GetPosts(new List<string> {_seenPosts[PostIndex]}).First();
            }

            var newPost = GetNewPost(GetSubreddit(reddit, Properties.SubredditName), Properties.Type, skipSeenPosts ? _seenPosts : null);
            _seenPosts.Add(newPost.Fullname);
            return newPost;
        }

        internal Post Previous(RedditAPI reddit)
        {
            if(PostIndex > 0) PostIndex--;
            return reddit.GetPosts(new List<string> {_seenPosts[PostIndex]}).First();
        }

        internal void ResetPostIndex()
        {
            PostIndex = _seenPosts.Count - 1;
        }
        
        private static Subreddit GetSubreddit(RedditAPI reddit, string subredditName) =>
            reddit.Subreddit(subredditName).About();

        private static Post GetNewPost(Subreddit subreddit, FeedType feedType, ICollection<string> seenPosts = null)
        {
            switch(feedType)
            {
                case FeedType.Hot:
                    return subreddit.Posts.Hot.SkipWhile(post => seenPosts == null || seenPosts.Contains(post.Fullname)).First();
                case FeedType.New:
                    return subreddit.Posts.New.SkipWhile(post => seenPosts == null || seenPosts.Contains(post.Fullname)).First();
                case FeedType.Rising:
                    return subreddit.Posts.Rising.SkipWhile(post => seenPosts == null || seenPosts.Contains(post.Fullname)).First();
                case FeedType.Controversial:
                    return subreddit.Posts.Controversial.SkipWhile(post => seenPosts == null || seenPosts.Contains(post.Fullname)).First();
                case FeedType.Top:
                    return subreddit.Posts.Top.SkipWhile(post => seenPosts == null || seenPosts.Contains(post.Fullname)).First();
                default:
                    return subreddit.Posts.Best.SkipWhile(post => seenPosts == null || seenPosts.Contains(post.Fullname)).First();
            }
        }

        public override string ToString() => Properties.ToString();
        
        public static FeedType GetFeedTypeFromChar(char c)
        {
            switch(char.ToLower(c))
            {
                case 'h':
                    return FeedType.Hot;
                case 'n':
                    return FeedType.New;
                case 'r':
                    return FeedType.Rising;
                case 'c':
                    return FeedType.Controversial;
                case 't':
                    return FeedType.Top;
                default:
                    return FeedType.Hot;
            }
        }

        internal enum FeedType
        {
            Hot,
            New,
            Rising,
            Controversial,
            Top
        }

        internal struct FeedProperties
        {
            internal readonly string SubredditName;
            internal readonly FeedType Type;

            internal FeedProperties(string subredditName, FeedType type = FeedType.Hot)
            {
                SubredditName = subredditName;
                Type = type;
            }

            public override bool Equals(object obj)
            {
                if(!(obj is FeedProperties))
                    return false;

                var other = (FeedProperties)obj;
                return SubredditName == other.SubredditName && Type == other.Type;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((SubredditName != null ? SubredditName.GetHashCode() : 0) * 397) ^ (int)Type;
                }
            }

            public override string ToString() => $"{SubredditName}/{Type:G}";
        }
    }
}