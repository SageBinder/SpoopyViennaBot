using System.Threading.Tasks;
using DSharpPlus.Entities;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Ephemeral
{
    internal class EnableEphemeralCommand : EphemeralBaseCommand
    {
        internal const string Trigger = "on";
        internal readonly string[] Triggers = {BaseTrigger, Trigger};
        
        internal const int DefaultDeleteDelay = 5; // in seconds
        
        internal EnableEphemeralCommand(EphemeralContext ephemeralContext) : base(ephemeralContext)
        {
        }

        protected override bool IsTriggeredByMessage(MessageContext context) => context.SatisfiesTriggers(Triggers);

        protected override async Task _Invoke(MessageContext context)
        {
            var args = context.GetSequentialArgs(Triggers.Length);
            int deleteDelay = (args.Length > 0 && int.TryParse(args[0], out deleteDelay))
                ? deleteDelay
                : DefaultDeleteDelay;
            
            var messageId = context.MessageEvent.Message.Id;
            DiscordMessage reply;
            
            var channelId = context.MessageEvent.Channel.Id;
            var wasEphemeral = EphemeralData.ContainsId(channelId);

            if(EphemeralData.Put(channelId, deleteDelay))
            {
                if(wasEphemeral)
                {
                    reply = await context.Reply($"Updated this channel's ephemeral delay, now {deleteDelay} seconds.").ConfigureAwait(false);
                }
                else
                {
                    reply = await context.Reply($"This channel is now ephemeral, delay is {deleteDelay} seconds.").ConfigureAwait(false);
                }
            }
            else
            {
                if(wasEphemeral)
                {
                    reply = await context.Reply(":x: Error: could not update this channel's ephemeral delay.").ConfigureAwait(false);
                }
                else
                {
                    reply = await context.Reply(":x: Error: could not mark this channel as ephemeral.").ConfigureAwait(false);
                }
            }

            EphemeralContext.NoDeleteMessageIdSet.Add(messageId);
            EphemeralContext.NoDeleteMessageIdSet.Add(reply.Id);
        }
    }
}