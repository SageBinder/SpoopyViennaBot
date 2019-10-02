using System.Threading.Tasks;
using System.Transactions;
using DSharpPlus.Entities;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Ephemeral
{
    internal class DisableEphemeralCommand : EphemeralBaseCommand
    {
        internal const string Trigger = "off";
        internal readonly string[] Triggers = {BaseTrigger, Trigger};

        internal DisableEphemeralCommand(EphemeralContext ephemeralContext) : base(ephemeralContext)
        {
        }

        protected override bool IsTriggeredByMessage(MessageContext context) =>
            context.SatisfiesTriggers(Triggers, canTakeArguments: false);

        protected override async Task _Invoke(MessageContext context)
        {
            var channelId = context.MessageEvent.Channel.Id;
            if(!EphemeralData.ContainsId(channelId))
            {
                await context.Reply("This channel is already non-ephemeral").ConfigureAwait(false);
                return;
            }
         
            var messageId = context.MessageEvent.Message.Id;
            DiscordMessage reply;
            
            if(EphemeralData.Remove(channelId))
            {
                reply = await context.Reply("This channel is no longer ephemeral.").ConfigureAwait(false);
            }
            else
            {
                reply = await context.Reply(":x: Error: could not mark this channel as non-ephemeral.").ConfigureAwait(false);
            }

            EphemeralContext.NoDeleteMessageIdSet.Add(messageId);
            EphemeralContext.NoDeleteMessageIdSet.Add(reply.Id);
        }
    }
}