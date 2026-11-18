using SmartEMR.ApiPractice.DTOs;

namespace SmartEMR.ApiPractice.Services;

public interface IReservationService
{
    ServiceResult<Reservation_Res> GetReservations();
    Task<ServiceResult<Reservation_Res>> SetReservation(Reservation_Req request, CancellationToken ct);
}
