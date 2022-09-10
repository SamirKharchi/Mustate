using Mustate.Boundary;
using Mustate.Boundary.Exceptions;
using Mustate.Internal.Objects;
using Mustate.UnitTests.Models;
using Shouldly;

namespace Mustate.UnitTests.Objects;

public class MutableStateTests
{
    #region Snapshot
    [Fact]
    public void Snapshot_ShouldNotHaveChanged()
    {
        // arrange
        var model = MutableGenerators.CreateModel();

        // act
        MutableState<SomeMutable>.Snapshot(model);

        // assert
        model.HasChanged().ShouldBeFalse();
    }

    [Fact]
    public void Snapshot_PropertyChangedToSameOrEmptyValue_ShouldNotHaveChanged()
    {
        // arrange
        var model = MutableGenerators.CreateModel();
        model.Snapshot();

        // act
        model.Mutable = null;
        model.NonMutable = "Changed";
        if (model.MutableFull is not null)
        {
            model.MutableFull.Mutable = null;
            model.MutableFull.NonMutable = "Changed";
        }

        // assert
        model.HasChanged().ShouldBeFalse();
    }

    [Fact]
    public void Snapshot_PropertyChanged_ShouldHaveChanged()
    {
        // arrange
        var model = MutableGenerators.CreateModel();
        model.Snapshot();

        // act
        model.Mutable = "Something";
        model.NonMutable = "Changed";

        // assert
        model.HasChanged().ShouldBeTrue();
    }

    [Fact]
    public void Snapshot_NestedPropertyChanged_ShouldHaveChanged()
    {
        // arrange
        var model = MutableGenerators.CreateModel();
        model.Snapshot();

        // act
        if (model.MutableFull is not null)
        {
            model.MutableFull.Mutable = "Something";
            model.MutableFull.NonMutable = "Changed";
        }

        // assert
        model.HasChanged().ShouldBeTrue();
    }
    #endregion

    #region IsRegistered
    [Fact]
    public void IsRegistered_Registered_ShouldBeTrue()
    {
        // arrange
        var model = MutableGenerators.CreateModel();
        model.Snapshot();
        
        // act & assert
        MustateApi.IsRegistered<SomeMutable>().ShouldBeTrue();
    }
    
    [Fact]
    public void IsRegisteredStatic_Unregistered_ShouldBeFalse()
    {
        // act & assert
        MustateApi.IsRegistered<SomeMutable>().ShouldBeFalse();
    }
    #endregion

    #region HasChanged
    [Fact]
    public void HasChanged_UnregisteredType_ShouldThrowUnregisteredTypeException()
    {
        // arrange
        var unregisteredType = new UnregisteredMutable();

        // act & assert
        Should.Throw<UnregisteredTypeException>(() => MutableState<UnregisteredMutable>.HasChanged(unregisteredType));
    }
    #endregion
}