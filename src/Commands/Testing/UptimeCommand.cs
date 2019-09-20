using System;
using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Testing
{
    public class UptimeCommand : Command
    {
        private readonly DateTime _startDate;
        private const string Trigger = "!uptime";

        public UptimeCommand(DateTime startDate)
        {
            _startDate = startDate;
        }
        
        public override bool IsTriggeredByMessage(CommandContext context)
        {
            return MessageMatchesTriggerList(context.Message, new[] {Trigger}, canTakeArguments: false);
        }

        public override async Task Invoke(CommandContext context)
        {
            await context.Reply(FormatTimeSpan(DateTime.UtcNow - _startDate).ToString("")
                                + " (since " + DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss") + "Z)");
        }

        private static TimeSpan FormatTimeSpan(TimeSpan timeSpan)
        {
            return new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}