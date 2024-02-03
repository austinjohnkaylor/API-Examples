using Bogus;
using InMemoryCaching.API.EntityFramework;

namespace InMemoryCaching.API;

/// <summary>
/// Class that generates fake people using the Bogus .NET Library
/// </summary>
public static class PersonGenerator
{
    public static readonly List<EntityFramework.Person> People = new();

    public static List<EntityFramework.Person> Generate(int numberOfPeople)
    {
        var id = 1;
        var personFaker = new Faker<EntityFramework.Person>()
            .RuleFor(p => p.Id, f => id++)
            .RuleFor(p => p.FirstName, f => f.Person.FirstName)
            .RuleFor(p => p.MiddleName, f => f.Name.Random.Word())
            .RuleFor(p => p.LastName, f => f.Person.LastName)
            .RuleFor(p => p.Age, f => f.Random.Int(10, 130))
            .RuleFor(p => p.Gender, f => f.PickRandom<Gender>());

        var fakePeople = personFaker.Generate(numberOfPeople);
        People.AddRange(fakePeople);
        return fakePeople;
    }
}