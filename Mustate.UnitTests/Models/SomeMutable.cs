using Mustate.Boundary.Attributes;
using Mustate.Boundary.Contracts;

namespace Mustate.UnitTests.Models;

public class SomeMutable : IMutable
{
    public string? NonMutable { get; set; }
    
    [Mutable]
    public string? Mutable { get; set; }
    
    [Mutable]
    public AnotherMutable? MutableFull { get; set; }
}