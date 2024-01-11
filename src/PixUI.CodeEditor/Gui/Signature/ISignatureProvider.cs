using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeEditor;

public interface ISignatureParameter
{
    string Name { get; }
    string Label { get; }
    string? Documentation { get; }
}

public interface ISignatureItem
{
    string Name { get; }
    string Label { get; }
    string? Documentation { get; }
    IEnumerable<ISignatureParameter> Parameters { get; }
}

public interface ISignatureResult
{
    int ActiveSignature { get; }
    int ActiveParameter { get; }
    IEnumerable<ISignatureItem> Signatures { get; }
}

public interface ISignatureProvider
{
    IEnumerable<char> TriggerCharacters { get; }

    Task<ISignatureResult?> ProvideSignatures(Document document, int offset);
}