using System.Threading.Tasks;
using System.Timers;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Ephemeral
{
    public class EphemeralDeleter : Command
    {
        public override bool IsTriggeredByMessage(CommandContext context)
        {
            return EphemeralData.ContainsId(context.MessageEvent.Channel.Id);
        }

        public override async Task Invoke(CommandContext context)
        {
            var channelId = context.MessageEvent.Channel.Id;
            if(EphemeralData.Get(channelId) == 0)
            {
                await context.MessageEvent.Message.DeleteAsync();
                return;
            }
            
            var deleteTimer = new Timer(EphemeralData.Get(channelId) * 1000);
            deleteTimer.Elapsed += (sender, args) => context.MessageEvent.Message.DeleteAsync();
            deleteTimer.AutoReset = false;
            deleteTimer.Enabled = true;
        }
    }
}