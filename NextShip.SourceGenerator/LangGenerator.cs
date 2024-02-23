using Microsoft.CodeAnalysis;

namespace NextShip.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public class LangGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
    }
}