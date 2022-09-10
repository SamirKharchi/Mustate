using System.Runtime.CompilerServices;
using Mapster;
using Mustate.Boundary.Contracts;
using Mustate.Boundary.Exceptions;
using Mustate.Internal.Utils;

// Making this class accessible in the unit test project.
[assembly: InternalsVisibleTo("Mustate.UnitTests")]

namespace Mustate.Internal.Objects;

/// <summary>
/// A lazy mutable state object in order to check if state has changed for an <see cref="IMutable"/> object.
/// </summary>
internal static class MutableState<T> where T : class, IMutable, new()
{
    #region [ApiInvisible]
    /// <summary>
    /// Lazy instance to capture a model state.
    /// </summary>
    private static T? model;
    #endregion

    /// <summary>
    /// Default static constructor.
    /// </summary>
    static MutableState()
    {
        // Excludes immutable property mappings that are irrelevant for the change state check
        TypeAdapterConfig<T, T>.NewConfig().Ignore(MutableUtils.AllImmutables<T>()).Compile();
    }

    /// <summary>
    /// Retrieves if the type has been already registered via <see cref="Snapshot"/>.
    /// </summary>
    /// <returns>true if registered, false otherwise.</returns>
    internal static bool IsRegistered() => model is not null;

    /// <summary>
    /// Takes a snapshot of the model state.
    /// </summary>
    /// <param name="modelState">The model instance.</param>
    internal static void Snapshot(T modelState)
    {
        if (IsRegistered())
        {
            modelState.Adapt(model);
            return;
        }

        model = modelState.Adapt<T>();
    }

    /// <summary>
    /// Checks if the registered model has changed.
    /// </summary>
    /// <param name="latestModelState">An instance reflecting the latest state of the model.</param>
    /// <returns>true if the model state has changed, false if not.</returns>
    /// <exception cref="UnregisteredTypeException">Thrown if model type has not been registered via <see cref="Snapshot"/>.</exception>
    internal static bool HasChanged(T latestModelState)
    {
        if (!IsRegistered())
        {
            throw new UnregisteredTypeException($"Type {typeof(T)} was not registered. Please call {nameof(Snapshot)} first.");
        }
        return !MutableEquality.Equals(latestModelState, model);
    }
}