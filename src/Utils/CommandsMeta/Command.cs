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
        
        public static bool MessageMatchesTriggerList(string message,
            string[] triggers,
            string delimRegex = @"[\s+]",
            bool caseSensitive = false,
            bool canTakeArguments = true)
        {
            string[] splitMessage = Regex.Split(caseSensitive ? message : message.ToLower(), delimRegex);
            if(splitMessage.Length < triggers.Length || (!canTakeArguments && splitMessage.Length > triggers.Length))
            {
                return false;
            }

            return !triggers.Where((word, i) => !(caseSensitive ? word : word.ToLower()).Equals(splitMessage[i])).Any();
        }
    }
}