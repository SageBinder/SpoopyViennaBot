using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Ephemeral
{
    public class EphemeralBaseCommand : Command
    {
        internal const string BaseTrigger = "!ephemeral";

        public override bool IsTriggeredByMessage(CommandContext context) =>
            context.SatisfiesTriggers(new[] {BaseTrigger}, canTakeArguments: false);

        public override async Task Invoke(CommandContext context)
        {
            DiscordChannel channel = context.MessageEvent.Channel;
            ulong id = channel.Id;

            try
            {
                if(EphemeralData.ContainsId(id))
                {
                    await context.Reply($"This channel is ephemeral; delay is {EphemeralData.Get(id)} seconds.");
                }
                else
                {
                    await context.Reply("This channel is not ephemeral.");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}