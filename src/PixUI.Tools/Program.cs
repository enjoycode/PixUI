// See https://aka.ms/new-console-template for more information

using PixUI.Tools.Icons;

using var fs1 = File.OpenWrite("MaterialIcons.cs");
using var output1 = new StreamWriter(fs1);
IconsCodeGenerator.Generate("MaterialIcons", "Material Icons",
    "PixUI.MaterialIcons", "MaterialIcons.woff2",
    "MaterialIcons-Regular.codepoints", output1);

using var fs2 = File.OpenWrite("MaterialIconsOutlined.cs");
using var output2 = new StreamWriter(fs2);
IconsCodeGenerator.Generate("MaterialIconsOutlined", "Material Icons Outlined",
    "PixUI.MaterialIconsOutlined", "MaterialIconsOutlined.woff2",
    "MaterialIconsOutlined-Regular.codepoints", output2);

Console.WriteLine("Done.");