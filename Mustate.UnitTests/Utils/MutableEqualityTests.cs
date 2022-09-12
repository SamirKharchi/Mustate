using Mustate.Boundary;
using Mustate.UnitTests.Models;
using Shouldly;

namespace Mustate.UnitTests.Utils;

public class MutableEqualityTests
{
    private readonly UserMutable userMutable;
                                                  
    private static bool CustomEqualityMutablePropertyCheck(object? xValue, object? yValue)
    {
        // If it's a list of strings
        if (xValue is List<string> xStringList)
        {
            var yStringList = yValue as List<string>;

            var sameFiles = xStringList.Count == yStringList?.Count;
            if (!sameFiles)
            {
                return false;
            }

            // We compare each individual string
            for (var i = 0; i < xStringList.Count; i++)
            {
                sameFiles = xStringList[i] == yStringList?[i];
                if (!sameFiles)
                {
                    return false;
                }
            }
        }
        // Otherwise if y is a list of string, then they must differ
        else if (yValue is List<string>)
        {
            return false;
        }

        return true;
    }

    public MutableEqualityTests()
    {
        userMutable = new UserMutable();
        userMutable.mutable.Add("a");
        userMutable.mutable.Add("b");

        if (!MustateApi.IsRegistered<UserMutable>())
        {
            userMutable.RegisterMutableCheckForType(CustomEqualityMutablePropertyCheck);
        }
    }
    
    [Fact]
    public void AddMutableEquality_ChangeListString_HasChangedShouldBeTrue()
    {
        userMutable.Snapshot();

        userMutable.mutable[0] = "c";
        userMutable.HasChanged().ShouldBeTrue();
    }
    
    [Fact]
    public void AddMutableEquality_AddString_HasChangedShouldBeTrue()
    {
        userMutable.Snapshot();

        userMutable.mutable.Add("c");
        userMutable.HasChanged().ShouldBeTrue();
    }
    
    [Fact]
    public void AddMutableEquality_HasChangedShouldBeFalse()
    {
        userMutable.Snapshot();
        userMutable.HasChanged().ShouldBeFalse();
    }
}