using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using SpoopyViennaBot.Utils;

namespace SpoopyViennaBot.Commands.Ephemeral
{
    [Serializable]
    internal static class EphemeralData
    {
        // This dict maps ChannelID => EphemeralLength
        private static Dictionary<ulong, int> _ephemeralMap;
        private const string MapResourceFileName = "ephemeral_map.bin";

        static EphemeralData()
        {
            if(Resources.ResourceExists(MapResourceFileName))
            {
                try
                {
                    using(var mapStream = Resources.OpenResourceRead(MapResourceFileName))
                    {
                        _ephemeralMap = (Dictionary<ulong, int>)(new BinaryFormatter().Deserialize(mapStream));
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    Console.Out.WriteLine($"Couldn't deserialize {MapResourceFileName}!\nUsing new dictionary instead.");
                    _ephemeralMap = new Dictionary<ulong, int>();
                }
            }
            else
            {
                _ephemeralMap = new Dictionary<ulong, int>();
                try
                {
                    Resources.CreateNew(MapResourceFileName).Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    Console.Out.WriteLine($"Couldn't create file {MapResourceFileName}!");
                }
            }
        }

        internal static bool Put(ulong channelId, int ephemeralLength)
        {
            _ephemeralMap[channelId] = ephemeralLength;

            if(TryMapWrite())
            {
                return true;
            }

            _ephemeralMap.Remove(channelId);
            return false;
        }

        internal static int Get(ulong channelId)
        {
            return _ephemeralMap[channelId];
        }
        
        internal static bool Remove(ulong channelId)
        {
            var savedVal = _ephemeralMap[channelId];
            _ephemeralMap.Remove(channelId);

            if(TryMapWrite())
            {
                return true;
            }

            _ephemeralMap[channelId] = savedVal;
            return false;
        }

        internal static bool ContainsId(ulong channelId)
        {
            return _ephemeralMap.ContainsKey(channelId);
        }
        
        private static bool TryMapWrite()
        {
            try
            {
                using(var outStream = Resources.OpenResourceWrite(MapResourceFileName))
                {
                    new BinaryFormatter().Serialize(outStream, _ephemeralMap);
                }
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Console.Out.WriteLine($"Couldn't serialize _ephemeralMap to {MapResourceFileName}!");
                return false;
            }
        }
    }
}