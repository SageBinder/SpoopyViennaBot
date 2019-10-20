using System;
using System.IO;
using System.Threading.Tasks;

namespace SpoopyViennaBot.Utils
{
    public static class Resources
    {
        public static readonly string ResourcesPath =
            Path.Combine(new[] {AppDomain.CurrentDomain.BaseDirectory, "../../../src/Resources/"});

        public static string ReadAllText(string resourceName)
        {
            return File.ReadAllText(GetResourcePath(resourceName));
        }

        public static Task<string> ReadAllTextAsync(string resourceName)
        {
            return File.ReadAllTextAsync(GetResourcePath(resourceName));
        }

        public static FileStream OpenResourceRead(string resourceName)
        {
            return File.OpenRead(GetResourcePath(resourceName));
        }

        public static FileStream OpenResourceWrite(string resourceName)
        {
            return File.OpenWrite(GetResourcePath(resourceName));
        }

        public static FileStream OpenResource(string resourceName, FileMode fileMode)
        {
            return File.Open(GetResourcePath(resourceName), fileMode);
        }

        public static bool ResourceExists(string resourceName)
        {
            return File.Exists(GetResourcePath(resourceName));
        }

        public static FileStream CreateNew(string resourceName)
        {
            return File.Create(GetResourcePath(resourceName));
        }

        public static string GetResourcePath(string resourceName)
        {
            return $"{ResourcesPath}{resourceName}";
        }
    }
}