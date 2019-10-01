using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using SpoopyViennaBot.Utils;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Main
{
    internal static class BotMain
    {
        private static DiscordClient _botClient;
        private static InvokableList _invokables;
        public static DateTime BotCreateTime;

        private static async Task Main(string[] args)
        {
            BotCreateTime = DateTime.UtcNow;
            
            Commands.Reddit.Reddit.EstablishApi();
            var redditApiTimer = new System.Timers.Timer(1000 * 60 * 50); // 50 minutes
            redditApiTimer.Elapsed += (sender, eventArgs) => Commands.Reddit.Reddit.EstablishApi();
            redditApiTimer.AutoReset = true;
            redditApiTimer.Enabled = true;
            
            _invokables = CommandsInitializer.GetCommandList();
            _botClient = new DiscordClient(new DiscordConfiguration
            {
                Token = Resources.ReadAllText("discord_token.txt"),
                TokenType = TokenType.Bot
            });

            _botClient.MessageCreated += async e => await _invokables.Propagate(new MessageContext(_botClient, e));

            await _botClient.ConnectAsync();
            
            Console.WriteLine("\n\nHewwo mastew UwU");
            Console.WriteLine("I'm weady fow input");
            
            await Task.Delay(-1);
        }
    }
}