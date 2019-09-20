using System;
using System.Collections.Generic;
using SpoopyViennaBot.Commands.Reddit;
using SpoopyViennaBot.Commands.Testing;

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
            commands.Add(new UptimeCommand(DateTime.UtcNow));
            
            // Reddit
            commands.Add(new GetAskRedditQuestionCommand());
            commands.Add(new InitRedditApiCommand());
            
            return commands;
        }
    }
}