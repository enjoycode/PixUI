using System.Collections.Generic;
using PixUI.Platform.Mac;

namespace PixUI.Demo.Mac;

internal static class MainClass
{
    private static void Main(string[] args)
    {
        MacApplication.Run(new DemoRoute());
    }
}