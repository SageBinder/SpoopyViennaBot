using System;
using System.Threading.Tasks;
using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Testing
{
    public class UptimeCommand : Command
    {
        internal const string Trigger = "!uptime";
        private readonly DateTime _startDate;

        public UptimeCommand(DateTime startDate) => _startDate = startDate;

        protected override bool IsTriggeredByMessage(MessageContext context) =>
            context.SatisfiesTriggers(new[] {Trigger}, canTakeArguments: false);

        protected override async Task _Invoke(MessageContext context)
        {
            var uptimeTimeSpan = DateTime.UtcNow - _startDate;
            await context.Reply($"{uptimeTimeSpan.Days} days, " + 
                                $"{uptimeTimeSpan.Hours} hours, " +
                                $"{uptimeTimeSpan.Minutes} minutes, " +
                                $"{uptimeTimeSpan.Seconds} seconds " +
                                $"(since {_startDate:yyyy-MM-dd hh:mm:ss}Z)");
        }
    }
}