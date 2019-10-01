using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Ephemeral
{
    internal class EphemeralBaseCommand : Command
    {
        internal const string BaseTrigger = "!ephemeral";

        internal readonly EphemeralContext EphemeralContext;
        
        internal EphemeralBaseCommand(EphemeralContext ephemeralContext)
        {
            this.EphemeralContext = ephemeralContext;
        }

        protected override bool IsTriggeredByMessage(MessageContext context) =>
            context.SatisfiesTriggers(new[] {BaseTrigger}, canTakeArguments: false);

        protected override async Task _Invoke(MessageContext context)
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