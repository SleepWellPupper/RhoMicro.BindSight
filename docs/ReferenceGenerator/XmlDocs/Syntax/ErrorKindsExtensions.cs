namespace ReferenceGenerator.XmlDocs;

internal static class ErrorKindsExtensions
{
    public static Boolean HasFlag(this DiagnosticKinds flags, DiagnosticKinds mask)
        => (flags & mask) == mask;

    public static Int32 GetBits(this DiagnosticKinds flags, Span<DiagnosticKinds> bits)
    {
        var flagsInt = (UInt32)flags;
        var count = 0;

        for (var i = 0; i < 32; i++)
        {
            if ((flagsInt >> i) << i == flagsInt)
                continue;

            bits[count++] = (DiagnosticKinds)(UInt32)i;
        }

        return count;
    }

    public static Int32 GetPopCount(this DiagnosticKinds diagnosticKinds)
        => global::System.Numerics.BitOperations.PopCount((UInt32)diagnosticKinds);
}
