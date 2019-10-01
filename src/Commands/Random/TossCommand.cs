using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Random
{
    public class TossCommand : Command
    {
        internal const string Trigger = "!toss";
        internal const short MaxTosses = 128;

        private static readonly System.Random Random = new System.Random();

        protected override bool IsTriggeredByMessage(MessageContext context) => context.SatisfiesTriggers(new[] {Trigger});

        protected override async Task _Invoke(MessageContext context)
        {
            var args = context.GetSequentialArgs(1);
            short numTosses = args.Length > 0 && short.TryParse(args[0], out numTosses) ? numTosses : (short)1;
            numTosses = numTosses > MaxTosses ? MaxTosses : numTosses;

            if(numTosses == 1)
            {
                await context.Reply($"```diff\n{(Random.Next(2) == 0 ? "- Heads" : "+ Tails")}```");
                return;
            }
            
            // If true, heads. If false, tails.
            var heads = new bool[numTosses];
            for(var i = 0; i < numTosses; i++)
            {
                heads[i] = Random.Next(2) == 0;
            }

            var outputString = "```diff\n";
            var totalHeads = 0;
            for(var i = 0; i < heads.Length; i++)
            {
                outputString += heads[i] ? $"- {i + 1}: Heads\n" : $"+ {i + 1}: Tails\n";
                totalHeads += heads[i] ? 1 : 0;
            }

            outputString += "----------";
            outputString += $"\n+ Total tails: {numTosses - totalHeads}";
            outputString += $"\n- Total heads: {totalHeads}";
            outputString += "```";
            await context.Reply(outputString);
        }
    }
}