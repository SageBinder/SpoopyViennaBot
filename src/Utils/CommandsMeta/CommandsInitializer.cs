using System;
using System.Collections.Generic;
using SpoopyViennaBot.Commands.Random;
using SpoopyViennaBot.Commands.Reddit;
using SpoopyViennaBot.Commands.Testing;
using SpoopyViennaBot.Main;

namespace SpoopyViennaBot.Utils.CommandsMeta
{
    public static class CommandsInitializer
    {
        public static List<Command> GetCommands()
        {
            var commands = new List<Command>();
            
            // Testing
            commands.Add(new TestPingCommand());
            commands.Add(new IncrementCommand());
            commands.Add(new UptimeCommand(BotMain.BotCreateTime));
            
            // Reddit
            commands.Add(new RedditBaseCommand());
            commands.Add(new GetPostCommand());
            commands.Add(new GetAskRedditQuestionCommand());
            commands.Add(new ForceInitRedditApiCommand());
            commands.Add(new RedditApiStatusCommand());
            
            // Random
            commands.Add(new RollCommand());
            
            return commands;
        }
    }
}