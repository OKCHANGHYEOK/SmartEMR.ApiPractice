using SmartEMR.ApiPractice.Entities;

namespace SmartEMR.ApiPractice.Repository;

public interface IReservationRepository
{
    Reservation? GetReservation(Reservation item);
    List<Reservation> GetReservations();
    Reservation? SetReservation(Reservation item);
    bool ExistsByPatientId(int PAT_Idx);
}
