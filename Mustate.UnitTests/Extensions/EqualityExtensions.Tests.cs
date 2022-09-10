using Mustate.Internal.Extensions;
using Shouldly;

namespace Mustate.UnitTests.Extensions;

public class EqualityExtensionsTests
{
    #region EqualsByNullAndEmpty
    [Theory]
    [InlineData(null, null)]
    [InlineData(null, "")]
    [InlineData("", null)]
    [InlineData("", "")]
    [InlineData("not empty", "not empty")]
    public void EqualsByNullAndEmpty_ShouldReturnTrue(string a, string b)
    {
        // act
        var result = a.EqualsByNullAndEmpty(b);

        // assert
        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null, "not empty")]
    [InlineData("not empty", "")]
    [InlineData("not empty", null)]
    [InlineData("a", "b")]
    public void EqualsByNullAndEmpty_ShouldReturnFalse(string a, string b)
    {
        // act
        var result = a.EqualsByNullAndEmpty(b);

        // assert
        result.ShouldBeFalse();
    }
    #endregion

    #region EqualsByNull{T}
    [Theory]
    [InlineData(null, null)]
    [InlineData(1, 1)]
    public void EqualsByNull_ShouldReturnTrue<T>(T? a, T? b)
    {
        // act
        var result = a.EqualsByNull(b);

        // assert
        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null, 1)]
    [InlineData(1, null)]
    [InlineData(3, 2)]
    public void EqualsByNull_ShouldReturnFalse<T>(T? a, T? b)
    {
        // act
        var result = a.EqualsByNull(b);

        // assert
        result.ShouldBeFalse();
    }
    #endregion
}