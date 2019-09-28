using System;
using System.Collections.Generic;
using SpoopyViennaBot.Commands.Ephemeral;
using SpoopyViennaBot.Commands.Random;
using SpoopyViennaBot.Commands.Reddit;
using SpoopyViennaBot.Commands.Testing;
using SpoopyViennaBot.Main;

namespace SpoopyViennaBot.Utils.CommandsMeta
{
    public static class CommandsInitializer
    {
        public static CommandList GetCommandList()
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
            commands.Add(new TossCommand());
            
            // Ephemeral (I KNOW IT'S BAD, BUT ORDER MATTERS HERE!!)
            commands.Add(new EphemeralBaseCommand());
            commands.Add(new DisableEphemeralCommand());
            commands.Add(new EphemeralDeleter());
            commands.Add(new EnableEphemeralCommand());

            return new CommandList(commands);
        }
    }
}