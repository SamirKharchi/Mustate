using Mustate.Boundary.Attributes;
using Mustate.Boundary.Contracts;

namespace Mustate.UnitTests.Models;

public class UserMutable : IMutable
{
    [Mutable] public List<string> mutable { get; set; } = new();
}