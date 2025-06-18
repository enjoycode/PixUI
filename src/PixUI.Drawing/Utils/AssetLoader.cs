using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PixUI;

public static class AssetLoader
{
    private static readonly Dictionary<string, Assembly> AsmCaches = new ();

    public static Stream? LoadAsStream(string assemblyName, string path)
    {
        //TODO:??Windows??
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var filePath = path;
            if (path == "MaterialIcons.woff2")
                filePath = "MaterialIcons.ttf";
            else if (path == "MaterialIconsOutlined.woff2")
                filePath = "MaterialIconsOutlined.otf";
            return File.OpenRead(filePath);
        }

        Assembly? asm;
        lock (AsmCaches)
        {
            if (!AsmCaches.TryGetValue(assemblyName, out asm))
            {
                asm = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(t => t.GetName().Name == assemblyName);
                if (asm != null)
                    AsmCaches.Add(assemblyName, asm);
            }
        }

        if (asm == null) return null;

        return asm.GetManifestResourceStream($"{assemblyName}.Assets.{path}");
    }
}