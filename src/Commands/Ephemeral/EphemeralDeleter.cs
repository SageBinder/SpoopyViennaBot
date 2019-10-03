using System.Threading.Tasks;
using System.Timers;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Ephemeral
{
    internal class EphemeralDeleter : Command
    {
        private readonly EphemeralContext _ephemeralContext;
        
        internal EphemeralDeleter(EphemeralContext ephemeralContext)
        {
            _ephemeralContext = ephemeralContext;
        }

        protected override bool IsTriggeredByMessage(MessageContext context)
        {
            return EphemeralData.ContainsId(context.MessageEvent.Channel.Id);
        }

        protected override async Task _Invoke(MessageContext context)
        {
            if(_ephemeralContext.NoDeleteMessageIdSet.Contains(context.MessageEvent.Message.Id))
            {
                _ephemeralContext.NoDeleteMessageIdSet.Remove(context.MessageEvent.Message.Id);
                return;
            }
            
            var channelId = context.MessageEvent.Channel.Id;
            if(EphemeralData.Get(channelId) == 0)
            {
                await context.MessageEvent.Message.DeleteAsync().ConfigureAwait(false);
                return;
            }
            
            var deleteTimer = new Timer(EphemeralData.Get(channelId) * 1000);
            deleteTimer.Elapsed += (sender, args) => context.MessageEvent.Message.DeleteAsync();
            deleteTimer.AutoReset = false;
            deleteTimer.Enabled = true;
        }
    }
}