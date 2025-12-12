namespace _04_custom_provider;

public class TextFileConfigurationProvider : ConfigurationProvider
{
    private readonly string filePath;
    public TextFileConfigurationProvider(string filePath) => this.filePath = filePath;

    public override void Load()
    {
        var data = new Dictionary<string, string?>();

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Configuration file not found: {filePath}");

        foreach(var line in File.ReadAllLines(filePath))
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith('#'))
                continue;
            
            var separatorIndex = trimmedLine.IndexOf('=');
            if (separatorIndex < 0)
                continue;

            var key = trimmedLine.Substring(0, separatorIndex).Trim();
            var value = trimmedLine.Substring(separatorIndex + 1).Trim();

            if (value.Length > 1 && value.StartsWith('"') && value.EndsWith('"'))
                value = value.Substring(1, value.Length - 2);

            data.Add(key, value);
        }

        Data = data;
    }
}
