using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Ephemeral
{
    public class EnableEphemeralCommand : Command
    {
        internal const string Trigger = "on";
        internal readonly string[] Triggers = {EphemeralBaseCommand.BaseTrigger, Trigger};
        
        internal const int DefaultDeleteDelay = 5; // in seconds
        
        public override bool IsTriggeredByMessage(CommandContext context) => context.SatisfiesTriggers(Triggers);

        public override async Task Invoke(CommandContext context)
        {
            var args = context.GetSequentialArgs(Triggers.Length);
            int deleteDelay = (args.Length > 0 && int.TryParse(args[0], out deleteDelay))
                ? deleteDelay
                : DefaultDeleteDelay;
            
            ulong channelId = context.MessageEvent.Channel.Id;
            bool wasEphemeral = EphemeralData.ContainsId(channelId);

            if(EphemeralData.Put(channelId, deleteDelay))
            {
                if(wasEphemeral)
                {
                    await context.Reply($"Updated this channel's ephemeral delay, now {deleteDelay} seconds.");
                }
                else
                {
                    await context.Reply($"This channel is now ephemeral, delay is {deleteDelay} seconds.");
                }
            }
            else
            {
                if(wasEphemeral)
                {
                    await context.Reply(":x: Error: could not update this channel's ephemeral delay.");
                }
                else
                {
                    await context.Reply(":x: Error: could not mark this channel as ephemeral.");
                }
            }
        }
    }
}