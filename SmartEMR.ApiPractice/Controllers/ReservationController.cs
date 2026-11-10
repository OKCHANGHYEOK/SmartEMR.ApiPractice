using Microsoft.AspNetCore.Mvc;
using SmartEMR.ApiPractice.DTOs;
using SmartEMR.ApiPractice.Services;

namespace SmartEMR.ApiPractice.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public IActionResult GetReservations()
    {
        var result = _reservationService.GetReservations();
        if (result.Items is null)
        {
            return NoContent();
        }

        return Ok(result.Items);
    }

    [HttpPost]
    public IActionResult SetReservation(Reservation_Req request)
    {
        var result = _reservationService.SetReservation(request);
        if (result.Item is null)
        {
            var response = new
            {
                Error = "예약 저장에 실패했습니다.",
                result.Message
            };

            return result.StatusCode switch
            {
                400 => BadRequest(response),
                404 => NotFound(response),
                409 => Conflict(response),
                _ => StatusCode(result.StatusCode, response)
            };
        }

        return CreatedAtAction(nameof(SetReservation), new { result.Item.RES_Idx}, result);
    }
}
