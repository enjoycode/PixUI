using System.Collections.Generic;
using PixUI.Platform.Mac;
using UniformTypeIdentifiers;

namespace PixUI.Demo.Mac;

internal static class MainClass
{
    private static void Main(string[] args)
    {
        MacApplication.Run(new DemoRoute());
    }
}