using System.Linq;
using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Testing
{
    public class TestPingCommand : Command
    {
        public override bool IsTriggeredByMessage(CommandContext context) =>
            context.MessageEvent.MentionedUsers.Contains(context.BotClient.CurrentUser);

        public override async Task Invoke(CommandContext context) => await context.Reply("Hello!");
    }
}