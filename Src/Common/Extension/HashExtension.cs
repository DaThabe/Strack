using System.Security.Cryptography;
using System.Text;

namespace Common.Extension;

public static class HashExtension
{
    /// <summary>
    /// 将对象转为字符串后转为Guid
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Guid ToHashedGuid(this object obj)
    {
        return obj.ToString().ToHashedGuid();
    }

    public static Guid ToHashedGuid(this string? str)
    {
        if (string.IsNullOrWhiteSpace(str)) throw new InvalidOperationException("无法生成Guid,对象无法转为含值字符串");

        var data = Encoding.UTF8.GetBytes(str);
        var hash = SHA1.HashData(data);

        // 使用前 16 字节构造 Guid（SHA1 输出是 20 字节）
        var guidBytes = new byte[16];
        Array.Copy(hash, guidBytes, 16);

        return new Guid(guidBytes);
    }
}
