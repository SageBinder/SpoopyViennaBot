using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using SpoopyViennaBot.Utils;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Main
{
    internal static class BotMain
    {
        private static DiscordClient _botClient;
        private static readonly Dictionary<ulong, InvokableList> _guildCommandsDict = new Dictionary<ulong, InvokableList>();
        public static DateTime BotCreateTime;

        private static async Task Main()
        {
            BotCreateTime = DateTime.UtcNow;
            
            Commands.Reddit.Reddit.EstablishApi();
            var redditApiTimer = new System.Timers.Timer(1000 * 60 * 50); // 50 minutes
            redditApiTimer.Elapsed += (sender, eventArgs) => Commands.Reddit.Reddit.EstablishApi();
            redditApiTimer.AutoReset = true;
            redditApiTimer.Enabled = true;
            
            _botClient = new DiscordClient(new DiscordConfiguration
            {
                Token = Resources.ReadAllText("discord_token.txt"),
                TokenType = TokenType.Bot
            });

            _botClient.MessageCreated += async e =>
            {
                if(!_guildCommandsDict.ContainsKey(e.Guild.Id))
                {
                    _guildCommandsDict[e.Guild.Id] = CommandsInitializer.GetCommandList();
                }
                await _guildCommandsDict[e.Guild.Id].Propagate(new MessageContext(_botClient, e)).ConfigureAwait(false);
            };

            await _botClient.ConnectAsync().ConfigureAwait(false);
            
            Console.WriteLine("\n\nHewwo mastew UwU");
            Console.WriteLine("I'm weady fow input");

            await Task.Delay(-1).ConfigureAwait(false);
        }
    }
}