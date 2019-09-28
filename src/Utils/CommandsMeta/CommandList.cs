using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpoopyViennaBot.Utils.CommandsMeta
{
    public class CommandList
    {
        private readonly List<Command> _commands;

        public CommandList(List<Command> commands)
        {
            _commands = commands;
        }

        public async Task Propagate(CommandContext context)
        {
            var invokeTasks = _commands.Select(command => command.InvokeIfMessageTriggers(context)).ToList();
            await Task.WhenAll(invokeTasks);
        }
    }
}