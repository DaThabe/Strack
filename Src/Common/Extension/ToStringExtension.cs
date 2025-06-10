using System.Text;

namespace Common.Extension;

public static class ToStringExtension
{
    public static ToStringBuilder ToStringBuilder(this object _)
    {
        return new ToStringBuilder();
    }
}


public class ToStringBuilder
{
    public ToStringBuilder AddParam(object? value, string name)
    {
        var valueContent = value?.ToString();
        if (!string.IsNullOrWhiteSpace(valueContent))
        {
            _source.Add($"{name}:{{{valueContent}}}");
        }

        return this;
    }

    public ToStringBuilder AddParam(IEnumerable<object?>? value, string name)
    {
        if (value == null) return this;

        var valueContent = string.Join(", ", value.Select(x => x?.ToString()).Where(x => !string.IsNullOrWhiteSpace(x)));
        if (!string.IsNullOrWhiteSpace(valueContent))
        {
            _source.Add($"{name}:{{{valueContent}}}");
        }

        return this;
    }

    public override string ToString()
    {
        return string.Join(',', _source);
    }


    private readonly List<string> _source = [];
}
