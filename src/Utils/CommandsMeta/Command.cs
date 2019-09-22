using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpoopyViennaBot.Utils.CommandsMeta
{
    public abstract class Command
    {
        public abstract bool IsTriggeredByMessage(CommandContext context);

        public abstract Task Invoke(CommandContext context);

        public async Task<bool> InvokeIfMessageTriggers(CommandContext context)
        {
            if(!IsTriggeredByMessage(context)) return false;
            await Invoke(context);
            return true;
        }
    }
}