using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Purge
{
    public class PurgeCommand : Command
    {
        internal const string Trigger = "!purge";
        internal const int MaxPurgeCount = 100;
        internal static List<ulong> PurgerIds = new List<ulong>();
        
        protected override bool IsTriggeredByMessage(MessageContext context) =>
            context.SatisfiesTriggers(new[] {Trigger}) &&
            (PurgerIds.Contains(context.Author.Id) || context.Author == context.MessageEvent.Guild.Owner);

        protected override async Task _Invoke(MessageContext context)
        {
            var args = context.GetSequentialArgs(1);
            if(!(args.Length > 0 && int.TryParse(args[0], out var purgeCount)))
            {
                await context.Reply("Usage: `!purge <num_messages_to_purge>`");
                return;
            }

            if(purgeCount > MaxPurgeCount) purgeCount = MaxPurgeCount;

            try
            {
                await context.MessageEvent.Message.DeleteAsync();
                for(var i = 0; i < purgeCount; i++)
                {
                    var messages =
                        await context.MessageEvent.Channel.GetMessagesAsync(1,
                            context.MessageEvent.Message.Id);
                    await context.MessageEvent.Channel.DeleteMessagesAsync(messages);
                }
            }
            catch(Exception)
            {
                await context.Reply("Error: couldn't purge messages.");
            }
        }
    }
}