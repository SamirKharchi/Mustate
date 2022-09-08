namespace Mustate.Boundary.Attributes;

/// <summary>
/// Flags a property as a mutable one in order to include it in mutable state equality.
/// </summary>
[AttributeUsage(AttributeTargets.Property|AttributeTargets.Class)]
public class MutableAttribute : Attribute
{
}