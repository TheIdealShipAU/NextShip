using Microsoft.CodeAnalysis;

namespace NextShip.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public class BuildInfoGenerator : IIncrementalGenerator
{
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
       var Time = DateTime.Now.ToString("G").Replace(" ", "-").Replace("/", "-");
    }
}