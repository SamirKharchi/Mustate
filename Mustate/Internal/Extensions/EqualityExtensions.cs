namespace Mustate.Internal.Extensions;

/// <summary>
/// Extension methods concerning equality operations.
/// </summary>
public static class EqualityExtensions
{
    /// <summary>
    /// Checks state equality of a string with another string.
    /// </summary>
    /// <param name="src">The left hand-side string.</param>
    /// <param name="other">The right hand-side string.</param>
    /// <returns>true if same string, both null or empty, false if they differ.</returns>
    public static bool EqualsByNullAndEmpty(this string? src, string? other)
    {
        return (src is null or { Length: 0 } && other is null or { Length: 0 }) || src == other;
    }
        
    /// <summary>
    /// Checks state equality of the object with another object.
    /// </summary>
    /// <param name="src">The left hand-side string.</param>
    /// <param name="other">The right hand-side string.</param>
    /// <returns>true if both are null or default equal, false if they differ.</returns>
    public static bool EqualsByNull<T>(this T? src, T? other)
    {
        // Checking for null equality here as a safe guard              
        return (src is null && other is null) || Equals(src, other);
    }
}