using System.Reflection;
using System.Runtime.CompilerServices;
using Mustate.Boundary.Attributes;
using Mustate.Boundary.Contracts;
using Mustate.Internal.Extensions;
using Mustate.Internal.Utils;

// Making this class accessible in the unit test project.
[assembly: InternalsVisibleTo("Mustate.UnitTests")]

namespace Mustate.Internal.Objects;

/// <summary>
/// Generically compares mutable property states for equality.
/// </summary>
internal static class MutableEquality
{
    #region [ApiInvisible]
    /// <summary>
    /// Retrieves the value of a given property via reflection.
    /// </summary>
    /// <param name="src">An instance of type T.</param>
    /// <param name="propName">The name of the property.</param>
    /// <typeparam name="T">The type that has <see cref="MutableAttribute"/> properties defined.</typeparam>
    /// <returns></returns>
    private static object? GetPropertyValue<T>(T? src, string propName) =>
            src?.GetType().GetProperty(propName)?.GetValue(src, null);

    /// <summary>
    /// Calls the <see cref="Equals{T}"/> method using reflection in order to evaluate the correct type parameter.
    /// </summary>
    /// <param name="x">The first parameter for the equality check.</param>
    /// <param name="y">The second parameter for the equality check. Should be of the same type as <see cref="x"/>.</param>
    /// <returns>true if they are equal, false if they differ and null if an error occurs.</returns>
    private static bool? CallEqualsByReflection(object x, object? y)
    {
        // Get the generic method definition
        var method = typeof(MutableEquality).GetMethod(nameof(Equals), BindingFlags.Public | BindingFlags.Static);

        // Build a method with the specific type argument and invoke
        return (bool?) method?.MakeGenericMethod(x.GetType()).Invoke(null, new[] {x, y});
    }

    /// <summary>
    /// Checks if all properties deriving from <see cref="IMutable"/> are equal.
    /// </summary>
    /// <param name="x">The first parameter for the equality check.</param>
    /// <param name="y">The second parameter for the equality check. Should be of the same type as <see cref="x"/>.</param>
    /// <typeparam name="T">The type that has <see cref="MutableAttribute"/> properties defined.</typeparam>
    /// <returns></returns>
    private static bool AreIMutablesEqual<T>(T? x, T? y) where T : class, IMutable
    {
        var nestedMutableProperties = MutableUtils.AllNestedMutables<T>();
        foreach (var nestedMutableProperty in nestedMutableProperties)
        {
            var xValue = GetPropertyValue(x, nestedMutableProperty);
            var yValue = GetPropertyValue(y, nestedMutableProperty);

            if (xValue is not null)
            {
                var isEqual = CallEqualsByReflection(xValue, yValue);
                if (isEqual is not null)
                {
                    return (bool) isEqual;
                }
            }
            else if (yValue is not null)
            {
                var isEqual = CallEqualsByReflection(yValue, xValue);
                if (isEqual is not null)
                {
                    return (bool) isEqual;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if mutable properties are equal.
    /// </summary>
    /// <param name="x">The first parameter for the equality check.</param>
    /// <param name="y">The second parameter for the equality check. Should be of the same type as <see cref="x"/>.</param>        
    /// <typeparam name="T">The type that has <see cref="MutableAttribute"/> properties defined.</typeparam>
    /// <returns></returns>
    private static bool AreMutablesEqual<T>(T? x, T? y) where T : class, IMutable, new()
    {
        var mutableProperties = MutableUtils.AllMutables<T>();
        foreach (var mutableProperty in mutableProperties)
        {
            var xValue = GetPropertyValue(x, mutableProperty);
            var yValue = GetPropertyValue(y, mutableProperty);

            if (!EqualsStringChecks(xValue, yValue, out var wasChecked))
            {
                return false;
            }

            if (!wasChecked && !EqualsUserChecks(xValue,yValue, MutableState<T>.UserCheck(), out wasChecked))
            {
                return false;
            }

            if (!wasChecked && !xValue.EqualsByNull(yValue))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if two mutable strings are equal.
    /// </summary>
    /// <param name="x">The first parameter for the equality check.</param>
    /// <param name="y">The second parameter for the equality check. Should be of the same type as <see cref="x"/>.</param>
    /// <param name="wasChecked">Is true if the the string check was performed, false if the input params are not strings.</param>
    /// <returns></returns>
    private static bool EqualsStringChecks(object? x, object? y, out bool wasChecked)
    {
        if (x is string xValueString)
        {
            wasChecked = true;
            // If x is a string use the string equality comparison
            if (!xValueString.EqualsByNullAndEmpty(y as string))
            {
                return false;
            }
        }
        else if (y is string yValueString)
        {
            wasChecked = true;
            // Otherwise if y is a string (x is implicitly null then)
            return yValueString is null or {Length: 0};
        }

        wasChecked = false;
        return true;
    }

    /// <summary>
    /// Checks if mutable user-defined type objects are equal.
    /// </summary>
    /// <param name="x">The first parameter for the equality check.</param>
    /// <param name="y">The second parameter for the equality check. Should be of the same type as <see cref="x"/>.</param>
    /// <param name="checkFunc"></param>
    /// <param name="wasChecked">Is true if the the user check was performed, false if the input params are not user-defined types.</param>
    /// <returns></returns>
    private static bool EqualsUserChecks(object? x, object? y, Func<object?, object?, bool>? checkFunc, out bool wasChecked)
    {
        if (checkFunc is null)
        {                     
            wasChecked = false;
            return true;
        }

        wasChecked = true;
        return checkFunc(x, y);

    }
    #endregion      

    /// <summary>
    /// Checks if all mutable properties of the given type have the same value.
    /// </summary>
    /// <param name="x">The first parameter for the equality check.</param>
    /// <param name="y">The second parameter for the equality check. Should be of the same type as <see cref="x"/>.</param>         
    /// <typeparam name="T">The model type that has <see cref="MutableAttribute"/> properties defined.</typeparam>
    /// <returns>true if mutable values do not differ, false otherwise.</returns>
    public static bool Equals<T>(T? x, T? y) where T : class, IMutable, new()
    {
        if (x is null && y is null)
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        return AreMutablesEqual(x, y) && AreIMutablesEqual(x, y);
    }
}