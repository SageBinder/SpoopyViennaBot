using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Random
{
    public class RollCommand : Command
    {
        private const string Trigger = "!roll";
        private static readonly System.Random Random = new System.Random();
        
        public override bool IsTriggeredByMessage(CommandContext context) =>
            context.SatisfiesTriggers(new[] {Trigger});

        public override async Task Invoke(CommandContext context)
        {
            var args = context.GetSequentialArgs(1);
            var messageString = "```diff\n";
            var totalRoll = 0;
            
            foreach(var arg in args)
            {
                if(!TryParseRollArg(arg, out var rollArg))
                {
                    messageString += $"- {arg}: {{Invalid Roll Syntax}}\n";
                    continue;
                }

                var sum = rollArg.Offset;
                for(var i = 0; i < rollArg.NumDie; i++)
                {
                    sum += Random.Next(rollArg.DieValue) + 1;
                }

                totalRoll += sum;
                messageString += $"+ {rollArg.NumDie}d{rollArg.DieValue}{(rollArg.Offset > 0 ? $"+{rollArg.Offset}" : "")}: {sum}\n";
            }

            if(args.Length > 1)
            {
                messageString += $"\nTotal: {totalRoll}";
            }
            messageString += "```";
            await context.Reply(messageString);
        }
        
        private static bool TryParseRollArg(string arg, out RollArg output)
        {
            // TODO: Implement negative offset
            var match = Regex.Match(arg, @"(?:([0-9]*)d)?([0-9]+)(?:\+([0-9]+))*");
            int numDie = int.TryParse(match.Groups[1].ToString(), out numDie) ? numDie : 1;
            int dieValue = int.TryParse(match.Groups[2].ToString(), out dieValue) ? dieValue : 20;
            int offset = int.TryParse(match.Groups[3].ToString(), out offset) ? offset : 0;

            output = new RollArg(numDie, dieValue, offset);
            return match.Success;
        }
        
        private class RollArg
        {
            internal int NumDie { get; }
            internal int DieValue { get; }
            internal int Offset { get;  }
            
            internal RollArg(int numDie, int dieValue, int offset = 0)
            {
                NumDie = numDie;
                DieValue = dieValue;
                Offset = offset;
            }
        }
    }
}