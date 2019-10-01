using SpoopyViennaBot.Utils.CommandsMeta;

namespace SpoopyViennaBot.Commands.Ephemeral
{
    public class EphemeralCommandGroup : CommandGroup
    {
        public EphemeralCommandGroup()
        {
            var ephemeralContext = new EphemeralContext(); 
            Commands.Add(new EphemeralBaseCommand(ephemeralContext));
            Commands.Add(new DisableEphemeralCommand(ephemeralContext));
            Commands.Add(new EphemeralDeleter(ephemeralContext));
            Commands.Add(new EnableEphemeralCommand(ephemeralContext));
        }
    }
}