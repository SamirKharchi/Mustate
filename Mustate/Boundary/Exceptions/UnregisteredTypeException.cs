using Mustate.Internal.Objects;

namespace Mustate.Boundary.Exceptions;

/// <summary>
/// Exception to be thrown when <see cref="MutableState{T}.HasChanged"/> is called on a type that has not
/// been registered by calling <see cref="MutableState{T}.Snapshot"/>. 
/// </summary>
public class UnregisteredTypeException : Exception
{
    public UnregisteredTypeException(string? message) : base(message)
    {
    }
}