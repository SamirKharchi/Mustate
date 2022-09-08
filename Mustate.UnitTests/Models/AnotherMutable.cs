using Mustate.Boundary.Attributes;
using Mustate.Boundary.Contracts;

namespace Mustate.UnitTests.Models;

public class AnotherMutable : IMutable
{
    public string? NonMutable { get; set; }
    [Mutable]
    public string? Mutable { get; set; }
}