using SmartEMR.ApiPractice.Entities;

namespace SmartEMR.ApiPractice.Repository;

public class ReservationRepository : IReservationRepository
{
    private IEnumerable<Reservation> _reservations =
    [
        new Reservation { RES_Idx = 1, PAT_Idx = 1, RES_Date = new DateTime(2026, 11, 11), RES_Route = "DSK", RES_Subject = "GNR"},
        new Reservation { RES_Idx = 2, PAT_Idx = 1, RES_Date = new DateTime(2027, 1, 15), RES_Route = "DSK", RES_Subject = "GNR"},
        new Reservation {RES_Idx = 3, PAT_Idx = 3, RES_Date = new DateTime(2026, 2, 18), RES_Route = "ONL", RES_Subject = "SKN"}
    ];

    public bool ExistsByPatientId(int PAT_Idx)
    {
        return _reservations.Any(x => x.PAT_Idx == PAT_Idx);
    }
}
