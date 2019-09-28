using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Testing
{
    public class IncrementCommand : Command
    {
        internal const string Trigger = "!inc";
        private int _val;
        public override bool IsTriggeredByMessage(CommandContext context) =>
            context.SatisfiesTriggers(new[] {Trigger});

        public override async Task Invoke(CommandContext context)
        {
            int oldVal = _val;
            string[] splitMessage = Regex.Split(context.MessageEvent.Message.Content, @"[\s+]");

            _val += (splitMessage.Length > 1 && int.TryParse(splitMessage[1], out var inc)) ? inc : 1;
            await context.Reply("Old val: " + oldVal + ", new val: " + _val);
        }
    }
}