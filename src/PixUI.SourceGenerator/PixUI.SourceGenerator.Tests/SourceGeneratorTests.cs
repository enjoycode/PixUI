using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using Xunit.Abstractions;

namespace PixUI.Tests;

public class SourceGeneratorTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    public SourceGeneratorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private const string StyleClassCode =
        """
        using PixUI;
        namespace TestNamespace;

        [StyleObject]
        public partial class ButtonStyle
        {
            [StyleProperty(defaultValue: "12f", validator: "{v} > 0 && {v} < 100", listener: "Relayout"]
            public partial BindableProperty<float> FontSize { get; set; }
        }";
        """;


    [Fact]
    public void GenerateStylePropertyTest()
    {
        // Create an instance of the source generator.
        var generator = new SourceGenerator();

        // Source generators should be tested using 'GeneratorDriver'.
        var driver = CSharpGeneratorDriver.Create(generator);

        // We need to create a compilation with the required source code.
        var compilation = CSharpCompilation.Create(nameof(SourceGeneratorTests),
            [CSharpSyntaxTree.ParseText(StyleClassCode)],
            [
                // To support 'System.Attribute' inheritance, add reference to 'System.Private.CoreLib'.
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            ]);

        // Run generators and retrieve all results.
        var runResult = driver.RunGenerators(compilation).GetRunResult();

        // All generated files can be found in 'RunResults.GeneratedTrees'.
        var generatedFileSyntax = runResult.GeneratedTrees.Single(t => t.FilePath.EndsWith("ButtonStyle.g.cs"));
        _testOutputHelper.WriteLine(generatedFileSyntax.GetText().ToString());

        // Complex generators should be tested using text comparison.
        // Assert.Equal(ExpectedGeneratedClassText, generatedFileSyntax.GetText().ToString(),
        //     ignoreLineEndingDifferences: true);
    }
}