using System.Collections.Generic;
using SpoopyViennaBot.Commands.Ephemeral;
using SpoopyViennaBot.Commands.Purge;
using SpoopyViennaBot.Commands.Random;
using SpoopyViennaBot.Commands.Reddit;
using SpoopyViennaBot.Commands.Testing;
using SpoopyViennaBot.Main;

namespace SpoopyViennaBot.Utils.CommandsMeta
{
    public static class CommandsInitializer
    {
        public static InvokableList GetCommandList()
        {
            var commands = new List<IInvokableMessageActor>();
            
            // Testing
            commands.Add(new TestPingCommand());
            commands.Add(new IncrementCommand());
            commands.Add(new UptimeCommand(BotMain.BotCreateTime));
            
            // Reddit
            commands.Add(new RedditCommandGroup());

            // Random
            commands.Add(new RollCommand());
            commands.Add(new TossCommand());
            
            // Ephemeral
            commands.Add(new EphemeralCommandGroup());

            // Purge
            commands.Add(new PurgeCommand());
            
            return new InvokableList(commands);
        }
    }
}