# Mustate
Mustate (Mutable State) is a small dotnet library which purpose is to allow snapshoting the state of a model's mutable properties and detect state changes with minimum effort.

## The Why
This arised from the requirement of form pages (e.g. when adding a product, editing person details or similar) to know when exactly the used model instance changes state on user input. 

While C#9 introduced records and value comparisons are much easier now, one still needs to implement equality members in order to exclude certain properties.

This is not only error prone (did you forget to include a property in the check?) and increases maintenance times (how will you enforce that you don't forget to handle a newly introduced property) but requires implementation for all affected models.

Furthermore, even value comparison is most of the time not enough, because usually when starting off with a null intitialised property (let's say a string), once the user inputs a value and clears it, the property might be an empty string (e.g. string.Empty or ""), which would mean the default value comparison detects a difference, while from the user's perspective nothing changed (the form field being empty is all (s)he sees).

Last but not least, not all code uses C#9 or records but instead needs to use classes (there are a bunch of reasons why this could be the case), so value comparison requires a mouthful of effort.

That's where Mustate can help avoid the aforementioned issues and reduce workload.

## The How
Mustate relies on a custom C# attribute, generics and reflection to semi-automate the process of detecting mutable state changes.

Imagine you have a mobile app page with a form and some fields (here dotnet MAUI).

#todo add image here

This form is bound to a model, which has only three properties of interest for the form.

What we do is mark the model as `IMutable` (for type safety reasons) and only give those properties a `Mutable` tag that are relevant for the form and/or which we want to detect state changes for.


```csharp
public class Person : IMutable
{
    public string Id { get; set; }

    [Mutable]
    public string First { get; set; }
    
    [Mutable]
    public string Last { get; set; }

    public string FullName => $"{First} {Last}";

    [Mutable]
    public Person Sibling { get; set; } 

    public Car RentedCar { get; set; }

    public List<string> Awards { get; set; }

    public string LinkToNirvana { get; set; }
}
```