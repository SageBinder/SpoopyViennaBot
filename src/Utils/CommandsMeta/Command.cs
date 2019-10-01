using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpoopyViennaBot.Utils.CommandsMeta
{
    public abstract class Command : IInvokableMessageActor
    {
        protected abstract bool IsTriggeredByMessage(MessageContext context);

        public async Task Invoke(MessageContext context)
        {
            if(IsTriggeredByMessage(context)) await _Invoke(context);
        }

        protected abstract Task _Invoke(MessageContext context);
    }
}