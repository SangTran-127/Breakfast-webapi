using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.Services.Breakfast;
using ErrorOr;
using BuberBreakfast.ServiceErrors;
using BuberBreakfast.Controllers;

namespace BubBreakfast.Controllers;

public class BreakfastController : ApiController
{

    private readonly IBreakfastService _breakfastService;

    public BreakfastController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
    }

    [HttpPost()]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {

        ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet
        );
        if (requestToBreakfastResult.IsError)
        {
            return Problem(requestToBreakfastResult.Errors);
        }
        var breakfast = requestToBreakfastResult.Value;
        // save to the database
        ErrorOr<Created> createBreakfastResult = _breakfastService.CreateBreakfast(breakfast);

        // if (createBreakfastResult.IsError)
        // {
        //     return Problem(createBreakfastResult.Errors);
        // }

        // return CreatedAtGetBreakfast(breakfast);
        return createBreakfastResult.Match(created => CreatedAtGetBreakfast(breakfast), errors => Problem(errors));

    }

    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);

        return getBreakfastResult.Match(breakfast => Ok(MapBreakfastResponse(breakfast)), errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        var requestToBreakfastResult = Breakfast.Create(
           request.Name,
           request.Description,
           request.StartDateTime,
           request.EndDateTime,
           request.Savory,
           request.Sweet,
            id
       );
        if (requestToBreakfastResult.IsError)
        {
            Problem(requestToBreakfastResult.Errors);
        }
        var breakfast = requestToBreakfastResult.Value;
        ErrorOr<UpsertBreakfastResult> upsertedResult = _breakfastService.UpsertBreakfast(breakfast);
        // return 201 if a new breakfast was created 
        return upsertedResult.Match(upserted => upserted.IsNewlyCreated ? CreatedAtGetBreakfast(breakfast) : NoContent(), errors => Problem(errors));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        ErrorOr<Deleted> deleteResult = _breakfastService.DeleteBreakfast(id);
        return deleteResult.Match(deleted => NoContent(), errors => Problem(errors));
    }


    private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new BreakfastResponse(
            breakfast.ID,
            breakfast.Name,
            breakfast.Description,
            breakfast.StartDateTime,
            breakfast.EndDateTime,
            breakfast.LastModifiedDateTime,
            breakfast.Savory,
            breakfast.Sweet
        );
    }
    private CreatedAtActionResult CreatedAtGetBreakfast(Breakfast breakfast)
    {
        return CreatedAtAction(nameof(GetBreakfast), new { id = breakfast.ID }, MapBreakfastResponse(breakfast));

    }
}