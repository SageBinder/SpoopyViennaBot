using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DSharpPlus;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Main
{
    internal static class BotMain
    {
        private static DiscordClient _botClient;
        private static List<Command> _commands;

        private static void Main(string[] args)
        {
            MainASync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task MainASync(string[] args)
        {
            Commands.Reddit.Reddit.EstablishApi();
            
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