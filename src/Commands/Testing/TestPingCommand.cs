using System.Linq;
using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Testing
{
    public class TestPingCommand : Command
    {
        protected override bool IsTriggeredByMessage(MessageContext context) =>
            context.MessageEvent.MentionedUsers.Contains(context.BotClient.CurrentUser);

        protected override async Task _Invoke(MessageContext context) => await context.Reply("Hello!");
    }
}