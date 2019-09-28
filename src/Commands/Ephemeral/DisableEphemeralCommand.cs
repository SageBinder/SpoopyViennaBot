using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Ephemeral
{
    public class DisableEphemeralCommand : Command
    {
        internal const string Trigger = "off";
        internal readonly string[] Triggers = {EphemeralBaseCommand.BaseTrigger, Trigger};
        
        public override bool IsTriggeredByMessage(CommandContext context) =>
            context.SatisfiesTriggers(Triggers, canTakeArguments: false);

        public override async Task Invoke(CommandContext context)
        {
            var channelId = context.MessageEvent.Channel.Id;
            if(!EphemeralData.ContainsId(channelId))
            {
                await context.Reply("This channel is already non-ephemeral");
                return;
            }
            
            if(EphemeralData.Remove(channelId))
            {
                await context.Reply("This channel is no longer ephemeral.");
            }
            else
            {
                await context.Reply(":x: Error: could not mark this channel as non-ephemeral.");
            }
        }
    }
}