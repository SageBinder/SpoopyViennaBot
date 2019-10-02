using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Random
{
    public class RollCommand : Command
    {
        internal const string Trigger = "!roll";
        private const string UsageString = @"
Usage examples:
`!roll 20`
`!roll 2d20`
`!roll 6+5`
`!roll 2d6+10`
`!roll d24-13`
";

        private static readonly System.Random Random = new System.Random();

        protected override bool IsTriggeredByMessage(MessageContext context) =>
            context.SatisfiesTriggers(new[] {Trigger});

        protected override async Task _Invoke(MessageContext context)
        {
            var args = context.GetSequentialArgs(1);
            if(args.Length == 0)
            {
                await context.Reply(UsageString).ConfigureAwait(false);
                return;
            }
            
            var messageString = "```diff\n";
            long totalRoll = 0;
            
            foreach(var arg in args)
            {
                if(!TryParseRollArg(arg, out var rollArg))
                {
                    messageString += $"- {arg}: {{Invalid Roll Syntax}}\n";
                    continue;
                }

                long sum = rollArg.Offset;
                for(var i = 0; i < rollArg.NumDie; i++)
                {
                    sum += Random.Next(rollArg.DieValue + 0) + 1;
                }

                totalRoll += sum;

                var offsetString =
                    rollArg.Offset > 0 ? $"+{rollArg.Offset}" :
                    rollArg.Offset < 0 ? $"{rollArg.Offset}" :
                    "";
                messageString += $"+ {rollArg.NumDie}d{rollArg.DieValue}{offsetString}: {sum}\n";
            }

            if(args.Length > 1)
            {
                messageString += $"\nTotal: {totalRoll}";
            }
            messageString += "```";
            await context.Reply(messageString).ConfigureAwait(false);
        }

        private static bool TryParseRollArg(string arg, out RollArg output)
        {
            // Matches something like "(2d)20(+5)"
            var match = Regex.Match(arg, @"(?:([0-9]*)d)?([0-9]+)(?:([\-\+])([0-9]+))?");
            short numDie = short.TryParse(match.Groups[1].ToString(), out numDie) ? numDie : (short)1;
            short dieValue = short.TryParse(match.Groups[2].ToString(), out dieValue) ? dieValue : (short)20;
            short offset = short.TryParse(match.Groups[4].ToString(), out offset) ? offset : (short)0;
            offset = (short)(match.Groups[3].Value.Equals("-") ? offset * (-1) : offset);

            output = new RollArg(numDie, dieValue, offset);
            return match.Success;
        }

        private class RollArg
        {
            internal short NumDie { get; }
            internal short DieValue { get; }
            internal short Offset { get; }
            
            internal RollArg(short numDie, short dieValue, short offset = 0)
            {
                NumDie = numDie;
                DieValue = dieValue;
                Offset = offset;
            }
        }
    }
}