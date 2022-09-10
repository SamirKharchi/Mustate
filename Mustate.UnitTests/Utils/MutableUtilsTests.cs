using Mustate.Internal.Utils;
using Mustate.UnitTests.Models;
using Shouldly;

namespace Mustate.UnitTests.Utils;

public class MutableUtilsTests
{
    [Fact]
    public void AllMutables_ShouldRetrieveCorrectProperties()
    {
        // act
        var allMutables = MutableUtils.AllMutables<SomeMutable>().ToList();

        // assert
        Assert.Multiple(
                () => allMutables.Count.ShouldBe(1),
                () => allMutables.ShouldContain(nameof(SomeMutable.Mutable))
                );
    }

    [Fact]
    public void AllNestedMutables_ShouldRetrieveCorrectProperties()
    {
        // act
        var allNestedMutables = MutableUtils.AllNestedMutables<SomeMutable>().ToList();

        // assert
        Assert.Multiple(
                () => allNestedMutables.Count.ShouldBe(1),
                () => allNestedMutables.ShouldContain(nameof(SomeMutable.MutableFull))
                );
    }

    [Fact]
    public void AllImmutables_ShouldRetrieveCorrectProperties()
    {
        // act
        var allImmutables = MutableUtils.AllImmutables<SomeMutable>().ToList();

        // assert
        Assert.Multiple(
                () => allImmutables.Count.ShouldBe(1),
                () => allImmutables.ShouldContain(nameof(SomeMutable.NonMutable))
                );
    }
}