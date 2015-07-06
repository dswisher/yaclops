using System;
using System.IO;

namespace Yaclops.Helpers
{
    internal static class ExeHelpers
    {
        public static string Name
        {
            get
            {
                // TODO - allow the name to be overridden in settings

                var path = Environment.GetCommandLineArgs()[0];
                if (string.IsNullOrEmpty(path))
                {
                    path = "Unknown";
                }

                return Path.GetFileNameWithoutExtension(path).ToLower();
            }
        }
    }
}
