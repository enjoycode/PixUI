using System;
using System.IO;
using System.Reflection;

namespace PixUI.UnitTests
{
    public static class Resources
    {
        private static readonly Assembly resAssembly = typeof(Resources).Assembly;

        public static string LoadString(string res)
        {
            using var stream = resAssembly.GetManifestResourceStream("PixUI.UnitTests." + res);
            if (stream == null)
                throw new Exception($"Can't find resource: {res}");
            
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}