using System.Text;

if (args.Length == 0)
{
    Console.WriteLine("Usage: WxmpPkgs [input path]");
    Environment.Exit(1);
}

var inputPath = args[0];
if (!Path.Exists(inputPath))
{
    Console.WriteLine("Input path does not exist.");
    Environment.Exit(2);
}

// create output directory
var outputPath = Path.Combine(inputPath, "pkgs");
if (Directory.Exists(outputPath))
    Directory.Delete(outputPath, true);
Directory.CreateDirectory(outputPath);

var compressedFiles = Directory.EnumerateFiles(inputPath, "*.br", SearchOption.TopDirectoryOnly)
    .Select(f => new FileInfo(f))
    .OrderByDescending(f => f.Length)
    .ToList();

const long maxPkgSize = 2 * 1024 * 1024;
const long jsFixSize = 25;

var pkgIndex = 1;

while (compressedFiles.Count > 0)
{
    var firstFile = compressedFiles[0];
    if (firstFile.Length >= maxPkgSize)
    {
        Console.WriteLine("File is too large.");
        Environment.Exit(3);
    }

    var pkgFiles = new List<FileInfo>();
    pkgFiles.Add(firstFile);
    compressedFiles.RemoveAt(0);
    var pkgLeftSize = maxPkgSize;
    pkgLeftSize -= firstFile.Length;
    pkgLeftSize -= jsFixSize;
    pkgLeftSize -= firstFile.Name.Length - 1;

    while (pkgLeftSize > 0)
    {
        var other = compressedFiles.FirstOrDefault(f => f.Length <= pkgLeftSize - f.Name.Length);
        if (other == null)
            break;

        pkgFiles.Add(other);
        compressedFiles.Remove(other);

        pkgLeftSize -= other.Length;
        pkgLeftSize -= other.Name.Length;
    }

    //创建输出目录
    var jsBuilder = new StringBuilder(1024);
    jsBuilder.Append("export const assets = [");
    var pkgPath = Path.Combine(outputPath, $"pkg{pkgIndex}");
    Directory.CreateDirectory(pkgPath);
    for (var i = 0; i < pkgFiles.Count; i++)
    {
        var pkgFile = pkgFiles[i];
        File.Copy(pkgFile.FullName, Path.Combine(outputPath, pkgPath, pkgFile.Name), true);
        if (i != 0) jsBuilder.Append(',');
        jsBuilder.Append($"\"{pkgFile.Name.AsSpan(0, pkgFile.Name.Length - 3)}\"");
    }

    jsBuilder.Append(']');
    File.WriteAllText(Path.Combine(outputPath, pkgPath, "index.js"), jsBuilder.ToString());

    pkgIndex++;
}