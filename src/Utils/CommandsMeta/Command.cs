using System.Threading.Tasks;

namespace SpoopyViennaBot.Utils.CommandsMeta
{
    public abstract class Command : IInvokableMessageActor
    {
        protected abstract bool IsTriggeredByMessage(MessageContext context);

        public async Task Invoke(MessageContext context)
        {
            if(IsTriggeredByMessage(context)) await _Invoke(context).ConfigureAwait(false);
        }

        protected abstract Task _Invoke(MessageContext context);
    }
}