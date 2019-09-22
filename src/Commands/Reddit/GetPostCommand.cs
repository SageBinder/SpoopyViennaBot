﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Reddit;
using Reddit.Controllers;
using Reddit.Inputs;
using Reddit.Inputs.Listings;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Reddit
{
    public class GetPostCommand : Command
    {
        private const string Trigger = "get";
        private static readonly string[] Triggers = {RedditBaseCommand.BaseTrigger, Trigger};

        public override bool IsTriggeredByMessage(CommandContext context) => context.SatisfiesTriggers(Triggers);

        public override async Task Invoke(CommandContext context)
        {
            var commandArgs = context.GetSequentialArgs(Triggers.Length);
            await _invoke(context, commandArgs);
        }

        internal static async Task _invoke(CommandContext context, string[] args)
        {
            if(args.Length == 0)
            {
                await context.Reply("Error: no arguments provided. Usage: `!reddit get <subreddit> [feed_type]`");
                return;
            }

            RedditAPI reddit;
            if((reddit = await RedditBaseCommand.EstablishApiIfNecessaryAndGet(context)) == null) return;

            var subredditName = args[0];
            Subreddit subreddit;
            try
            {
                subreddit = reddit.Subreddit(subredditName).About();
            }
            catch(Exception)
            {
                await context.Reply(
                    $"Error: couldn't access the subreddit *r/{subredditName}*. Are you sure it exists?");
                return;
            }

            Post latestPost;
            var feedType = "top";
            if(args.Length >= 2)
            {
                feedType = args[1];
            }

            // TODO: Extract this into a method when it needs to be reused
            if(char.ToLower(feedType[0]) == 'b')
                latestPost = subreddit.Posts.GetBest(new CategorizedSrListingInput(count: 1)).First();
            else if(char.ToLower(feedType[0]) == 'h')
                latestPost = subreddit.Posts.GetHot(new ListingsHotInput(count: 1)).First();
            else if(char.ToLower(feedType[0]) == 'n')
                latestPost = subreddit.Posts.GetNew(new CategorizedSrListingInput(count: 1)).First();
            else if(char.ToLower(feedType[0]) == 'r')
                latestPost = subreddit.Posts.GetRising(new CategorizedSrListingInput(count: 1)).First();
            else if(char.ToLower(feedType[0]) == 'c')
                latestPost = subreddit.Posts.GetControversial(new TimedCatSrListingInput(count: 1)).First();
            else if(char.ToLower(feedType[0]) == 't')
                latestPost = subreddit.Posts.GetTop().First();
            else
                latestPost = subreddit.Posts.GetNew(new CategorizedSrListingInput(count: 1)).First();

            await context.Reply(
                $"r/{subreddit.Name}: *\"{latestPost.Title}\"*\n" +
                $"({latestPost.UpVotes} upvote{(latestPost.UpVotes != 1 ? "s" : "")}, " +
                $"{latestPost.DownVotes} downvote{(latestPost.DownVotes != 1 ? "s" : "")})\n" +
                $"{latestPost.Listing.URL}");
        }
    }
}