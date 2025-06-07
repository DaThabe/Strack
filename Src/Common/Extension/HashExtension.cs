using System.Security.Cryptography;
using System.Text;

namespace Common.Extension;

public static class HashExtension
{
    public static Guid CombineToGuid(params object[] parameters)
    {
        var input = string.Join("|", parameters.Select(p => p?.ToString() ?? "null"));
        return CreateDeterministicGuid(_namespaceGuid, input);
    }

    private static Guid CreateDeterministicGuid(Guid namespaceId, string name)
    {
        // Convert namespace Guid to bytes (big-endian)
        byte[] namespaceBytes = namespaceId.ToByteArray();
        SwapGuidByteOrder(namespaceBytes);

        // Combine namespace and name bytes
        byte[] nameBytes = Encoding.UTF8.GetBytes(name);
        byte[] data = namespaceBytes.Concat(nameBytes).ToArray();

        // Hash with SHA1
        byte[] hash = SHA1.HashData(data);

        // Create GUID from first 16 bytes of hash
        hash[6] = (byte)(hash[6] & 0x0F | 0x50); // Version 5
        hash[8] = (byte)(hash[8] & 0x3F | 0x80); // Variant RFC4122

        SwapGuidByteOrder(hash); // back to little-endian

        return new Guid([.. hash.Take(16)]);
    }

    private static void SwapGuidByteOrder(byte[] guid)
    {
        // Guid bytes order fix for SHA compatibility (RFC 4122)
        Array.Reverse(guid, 0, 4);
        Array.Reverse(guid, 4, 2);
        Array.Reverse(guid, 6, 2);
    }


    private static readonly Guid _namespaceGuid = Guid.Parse("e1a19c10-456b-4f6d-9c2b-dcdd68b38e8b");
}
