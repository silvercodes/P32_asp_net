using System.Runtime.CompilerServices;

namespace _04_custom_provider;

public static class TextFileConfigurationExtensions
{
    public static IConfigurationBuilder AddTextFile(this IConfigurationBuilder builder, string filePath)
        => builder.Add(new TextFileConfigurationSource(filePath));
}
