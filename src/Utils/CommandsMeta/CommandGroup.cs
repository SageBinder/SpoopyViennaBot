using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpoopyViennaBot.Utils.CommandsMeta
{
    public abstract class CommandGroup : IInvokableMessageActor
    {
        protected readonly List<Command> Commands = new List<Command>();

        public async Task Invoke(MessageContext context) =>
            await Task.WhenAll(Commands.Select(command => command.Invoke(context)).ToList());
    }
}