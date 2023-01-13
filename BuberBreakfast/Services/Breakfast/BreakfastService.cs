

namespace BuberBreakfast.Services.Breakfast;

using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

public class BreakfastService : IBreakfastService
{
    private static readonly Dictionary<Guid, Breakfast> _breakfast = new();
    public ErrorOr<Created> CreateBreakfast(Breakfast breakfast)
    {
        _breakfast.Add(breakfast.ID, breakfast);
        return Result.Created;
    }
    public ErrorOr<Breakfast> GetBreakfast(Guid id)
    {
        if (_breakfast.TryGetValue(id, out var breakfast))
        {
            return breakfast;
        }
        return Errors.Breakfast.NotFound;


    }
    public ErrorOr<UpsertBreakfastResult> UpsertBreakfast(Breakfast breakfast)
    {
        var IsNewlyCreated = !_breakfast.ContainsKey(breakfast.ID);
        _breakfast[breakfast.ID] = breakfast;
        return new UpsertBreakfastResult(IsNewlyCreated);
    }

    public ErrorOr<Deleted> DeleteBreakfast(Guid id)
    {
        _breakfast.Remove(id);
        return Result.Deleted;
    }

}