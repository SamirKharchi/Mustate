using Mustate.Boundary.Contracts;
using Mustate.Internal.Objects;

namespace Mustate.Boundary;

/// <summary>
/// Public interface to handle mutable states for an object.
/// </summary>
public static class MustateApi
{
    /// <summary>
    /// Registers a model object for mutable state changes by taking a snapshot of its current state.
    /// </summary>
    /// <param name="model">The model instance.</param>
    /// <typeparam name="T">The type which must be derived from <see cref="IMutable"/></typeparam>
    public static void Snapshot<T>(this T model) where T : class, IMutable, new() => MutableState<T>.Snapshot(model);

    /// <summary>
    /// Checks if a type has already been registered for taking part in mutable state change checks. 
    /// </summary>
    /// <typeparam name="T">The type which must be derived from <see cref="IMutable"/></typeparam>
    /// <returns>true if registered, false otherwise.</returns>
    public static bool IsRegistered<T>() where T : class, IMutable, new() => MutableState<T>.IsRegistered();

    /// <summary>
    /// Checks if the registered model has changed.
    /// </summary>
    /// <param name="model">The model instance.</param>
    /// <typeparam name="T">The type which must be derived from <see cref="IMutable"/></typeparam>
    /// <returns>true if state changed, false otherwise.</returns>
    public static bool HasChanged<T>(this T model) where T : class, IMutable, new() =>
        MutableState<T>.HasChanged(model);

    /// <summary>
    /// Registers another mutable equality check.
    /// </summary>
    /// <param name="check">The equality check function.</param>
    /// <typeparam name="T">The type to add the check for.</typeparam>
    public static void RegisterMutableCheckForType<T>(this T _, Func<object?, object?, bool> check)
        where T : class, IMutable, new() => MutableState<T>.AddMutableEquality(check);
}