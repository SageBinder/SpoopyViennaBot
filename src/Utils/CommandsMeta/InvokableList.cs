using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpoopyViennaBot.Utils.CommandsMeta
{
    public class InvokableList
    {
        private readonly List<IInvokableMessageActor> _commands;

        public InvokableList(List<IInvokableMessageActor> commands)
        {
            _commands = commands;
        }

        public async Task Propagate(MessageContext context) =>
            await Task.WhenAll(_commands.Select(command => command.Invoke(context)).ToList()).ConfigureAwait(false);
    }
}