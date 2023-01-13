using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Models;

public class Breakfast
{

    public const int MIN_NAME_LENGTH = 3;
    public const int MAX_NAME_LENGTH = 50;

    public const int MIN_DESCRIPTION_LENGTH = 3;
    public const int MAX_DESCRIPTION_LENGTH = 50;

    public Guid ID { get; }

    public string Name { get; }

    public string Description { get; }

    public DateTime StartDateTime { get; }

    public DateTime EndDateTime { get; }

    public DateTime LastModifiedDateTime { get; }
    public List<string> Savory { get; }

    public List<string> Sweet { get; }
    private Breakfast(
        Guid id, string name, string description,
         DateTime startTime, DateTime lastModified, DateTime endTime, List<string> savory,
         List<string> sweet
        )
    {
        this.ID = id;
        this.Name = name;
        this.Description = description;
        this.StartDateTime = startTime;
        this.EndDateTime = endTime;
        this.LastModifiedDateTime = lastModified;
        this.Savory = savory;
        this.Sweet = sweet;
    }


    public static ErrorOr<Breakfast> Create(string name, string description,
         DateTime startTime, DateTime endTime, List<string> savory,
         List<string> sweet, Guid? id = null)
    {
        List<Error> errors = new List<Error>();
        if (name.Length < MIN_NAME_LENGTH || name.Length > MAX_NAME_LENGTH)
        {
            errors.Add(Errors.Breakfast.InvalidName);
        }


        if (errors.Count > 0)
        {
            return errors;
        }
        var breakfast = new Breakfast(
            id ?? Guid.NewGuid(),
            name,
            description,
            startTime,
            DateTime.UtcNow,
            endTime,
            savory,
            sweet
        );
        return breakfast;
    }

}