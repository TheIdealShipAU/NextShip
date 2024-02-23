#nullable enable
using System.Reflection;
using YamlDotNet.RepresentationModel;

namespace NextShip.Api.Utilities;

public class YamlLoader
{
    public static readonly List<YamlLoader> YamlLoaders = new();
    private readonly string FileName;
    public bool loaded;
    private YamlStream YamlStream;
    private string YamlTexts;

    public YamlLoader(string fileName)
    {
        FileName = fileName;
        YamlStream = new YamlStream();
        YamlTexts = string.Empty;
        YamlLoaders.Add(this);
    }

    public YamlLoader LoadFromDisk(string? path = null)
    {
        var FilePath = path;

        if (FilePath != null)
        {
            using var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            YamlStream = new YamlStream();
            var textReader = new StreamReader(fs);
            YamlTexts = textReader.ReadToEnd();
            YamlStream.Load(textReader);
        }

        loaded = true;
        return this;
    }

    public YamlLoader LoadFromResource(Assembly assembly)
    {
        var yaml = assembly.GetManifestResourceNames().FirstOrDefault(n => n == FileName + ".yaml");
        if (string.IsNullOrEmpty(yaml)) return this;
        var stream = assembly.GetManifestResourceStream(yaml);
        var textReader = new StreamReader(stream!);
        YamlTexts = textReader.ReadToEnd();
        YamlStream.Load(textReader);

        loaded = true;
        return this;
    }

    public YamlLoader LoadFromString(string yamlString)
    {
        YamlTexts = yamlString;
        var stream = new StringReader(yamlString);
        YamlStream.Load(stream);
        loaded = true;
        return this;
    }

    // https://github.com/CognifyDev/ClashOfGods/blob/main/COG/Utils/Yaml.cs
    /// <summary>
    ///     获取Int值
    /// </summary>
    /// <param name="location">路径，例如: GetInt("abab.sb"),"."表示下级</param>
    /// <returns>Int值</returns>
    public int? GetInt(string location)
    {
        int? result = null;
        var str = GetString(location);
        if (str != null && int.TryParse(str, out var i)) result = i;

        return result;
    }

    public List<string>? GetStringList(string location)
    {
        var locations = location.Contains('.') ? location.Split(".") : new[] { location };
        var rootNode = (YamlMappingNode)YamlStream.Documents[0].RootNode;

        if (locations.Length < 1) return new List<string>();

        YamlNode? valueNode = rootNode;
        foreach (var loc in locations)
            try
            {
                if (valueNode is not YamlMappingNode mappingNode) return null;

                var keyNode = new YamlScalarNode(loc);
                if (mappingNode.Children.TryGetValue(keyNode, out valueNode))
                    // 继续向下查找
                    continue;

                // 如果找不到对应的键或节点不是一个映射节点，则返回空列表
                return null;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }

        // 如果值节点是一个列表节点，则将列表中的值添加到结果列表中
        if (valueNode is not YamlSequenceNode sequenceNode) return new List<string>();
        var result = new List<string>();
        foreach (var item in sequenceNode.Children)
            if (item is YamlScalarNode { Value: not null } scalarNode)
                result.Add(scalarNode.Value);
        return result;

        // 如果值节点不是列表节点，则返回空列表
    }

    public byte? GetByte(string location)
    {
        var str = GetString(location);
        if (str == null) return null;
        if (byte.TryParse(str, out var result)) return result;

        return null;
    }

    public string? GetString(string location)
    {
        var locations = location.Contains('.') ? location.Split(".") : new[] { location };
        var rootNode = (YamlMappingNode)YamlStream.Documents[0].RootNode;

        if (locations.Length < 1) return null;

        YamlNode? valueNode = rootNode;
        foreach (var loc in locations)
            try
            {
                if (valueNode is not YamlMappingNode mappingNode) return null;

                var keyNode = new YamlScalarNode(loc);
                if (mappingNode.Children.TryGetValue(keyNode, out valueNode))
                    // 继续向下查找
                    continue;

                // 如果找不到对应的键或节点不是一个映射节点，则返回 null
                return null;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }

        return $"{valueNode}";
    }
}