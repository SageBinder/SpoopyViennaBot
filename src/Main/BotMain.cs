using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Main
{
    internal static class BotMain
    {
        private static DiscordClient _botClient;
        private static List<Command> _commands;
        public static DateTime BotCreateTime;

        private static async Task Main(string[] args)
        {
            BotCreateTime = DateTime.UtcNow;
            
            var redditApiTimer = new Timer(state => { Commands.Reddit.Reddit.EstablishApi(); },
                null,
                new TimeSpan(0, 0, 0),
                new TimeSpan(0, 50, 0));
            
            _commands = CommandsInitializer.GetCommands();
            _botClient = new DiscordClient(new DiscordConfiguration
            {
                Token = File.ReadAllText("../../../src/Resources/discord_token.txt"),
                TokenType = TokenType.Bot
            });

            _botClient.MessageCreated += async e =>
            {
                var commandContext = new CommandContext(_botClient, e);
                var invokeTasks = _commands.Select(command => command.InvokeIfMessageTriggers(commandContext)).ToList();
                await Task.WhenAll(invokeTasks);
            };

            await _botClient.ConnectAsync();
            
            Console.WriteLine("\n\nHewwo mastew UwU");
            Console.WriteLine("I'm weady fow input");
            
            await Task.Delay(-1);
        }
    }
}