using System.Reflection;
using Mustate.Boundary.Attributes;
using Mustate.Boundary.Contracts;

namespace Mustate.Internal.Utils;

/// <summary>
/// Utility functions for mutable properties.
/// </summary>
internal static class MutableUtils
{
    #region [ApiInvisible]
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static IEnumerable<PropertyInfo> GetPropertiesWithoutMutableAttribute<T>() where T : class
    {
        return typeof(T).GetProperties().Where(prop => !prop.IsDefined(typeof(MutableAttribute), false));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static IEnumerable<PropertyInfo> GetPropertiesWithMutableAttribute<T>(bool nestedMutables) where T : class
    {
        return typeof(T).GetProperties().Where(prop =>
        {
            var attribute = prop.GetCustomAttribute<MutableAttribute>();
            var exists = attribute is not null;
            var isIMutable = prop.PropertyType.GetInterface(nameof(IMutable)) is not null;
            // Attribute must be there and..
            if (nestedMutables)
            {
                // .. is an IMutable
                return exists && isIMutable;
            }

            // .. is not an IMutable
            return exists && !isIMutable;
        });
    }
    #endregion

    /// <summary>
    /// Returns an array of property names that are marked with the <see cref="MutableAttribute"/>.
    /// </summary>
    /// <typeparam name="T">Type of the model to get the mutable properties for.</typeparam>
    /// <returns>An array of strings containing property names.</returns>
    public static IEnumerable<string> AllMutables<T>() where T : class
    {
        var properties = GetPropertiesWithMutableAttribute<T>(false);
        return properties.Select(info => info.Name).ToArray();
    }

    /// <summary>
    /// Returns an array of property names that are marked with the <see cref="MutableAttribute"/> and which
    /// are themselves derived from IMutable.
    /// </summary>
    /// <typeparam name="T">Type of the model to get the mutable properties for.</typeparam>
    /// <returns>An array of strings containing property names.</returns>
    public static IEnumerable<string> AllNestedMutables<T>() where T : class, IMutable
    {
        var properties = GetPropertiesWithMutableAttribute<T>(true);
        return properties.Select(info => info.Name).ToArray();
    }

    /// <summary>
    /// Returns an array of property names that are not marked with the <see cref="MutableAttribute"/>.
    /// </summary>
    /// <typeparam name="T">Type of the model to get the immutable properties for.</typeparam>
    /// <returns>An array of strings containing property names.</returns>
    public static string[] AllImmutables<T>() where T : class
    {
        var properties = GetPropertiesWithoutMutableAttribute<T>();
        return properties.Select(info => info.Name).ToArray();
    }
}