using System.Text;

namespace PixUI.Tools.Icons
{
    public static class IconsCodeGenerator
    {
        public static void Generate(string className, string fontFamily, string asmName,
            string assetPath,
            string codepoints, TextWriter output)
        {
            output.WriteLine("namespace PixUI");
            output.WriteLine("{");

            output.WriteLine($"    public static class {className}");
            output.WriteLine("    {");

            output.WriteLine($"        private static readonly IconAsset _asset = new (\"{fontFamily}\", \"{asmName}\", \"{assetPath}\");");

            var asm = typeof(IconsCodeGenerator).Assembly;
            using var stream =
                asm.GetManifestResourceStream($"PixUI.Tools.MaterialIcons.{codepoints}");
            using var reader = new StreamReader(stream!);

            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                    break;

                var whiteSpace = line.AsSpan().IndexOf(' ');
                output.Write("        public static IconData ");
                WriteName(line.AsSpan(0, whiteSpace), output);
                output.Write(" => new IconData(0x");
                output.Write(line.AsSpan(whiteSpace + 1));
                output.WriteLine(", _asset);");
            }

            output.WriteLine("    }");
            output.WriteLine("}");
        }

        private static void WriteName(ReadOnlySpan<char> nameSpan, TextWriter output)
        {
            if (char.IsNumber(nameSpan[0]))
                output.Write('N');

            var toUpper = true;
            foreach (var c in nameSpan)
            {
                if (c == '_')
                {
                    toUpper = true;
                    continue;
                }

                output.Write(toUpper ? char.ToUpper(c) : c);
                if (toUpper) toUpper = false;
            }
        }
    }
}