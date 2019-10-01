using System;
using System.IO;

namespace SpoopyViennaBot.Utils
{
    public static class Resources
    {
        public static readonly string ResourcesPath =
            Path.Combine(new[] {AppDomain.CurrentDomain.BaseDirectory, "../../../src/Resources/"});

        public static string ReadAllText(string resourceName)
        {
            return File.ReadAllText($"{ResourcesPath}{resourceName}");
        }

        public static FileStream OpenResourceRead(string resourceName)
        {
            return File.OpenRead($"{ResourcesPath}{resourceName}");
        }

        public static FileStream OpenResourceWrite(string resourceName)
        {
            return File.OpenWrite($"{ResourcesPath}{resourceName}");
        }

        public static FileStream OpenResource(string resourceName, FileMode fileMode)
        {
            return File.Open($"{ResourcesPath}{resourceName}", fileMode);
        }

        public static bool ResourceExists(string resourceName)
        {
            return File.Exists($"{ResourcesPath}{resourceName}");
        }

        public static FileStream CreateNew(string resourceName)
        {
            return File.Create($"{ResourcesPath}{resourceName}");
        }
    }
}