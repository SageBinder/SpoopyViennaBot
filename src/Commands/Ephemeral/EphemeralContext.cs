using System.Collections.Generic;

namespace SpoopyViennaBot.Commands.Ephemeral
{
    internal class EphemeralContext
    {
        internal readonly HashSet<ulong> NoDeleteMessageIdSet = new HashSet<ulong>();
    }
}