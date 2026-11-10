using SmartEMR.ApiPractice.Entities;

namespace SmartEMR.ApiPractice.Repository;

public class ReservationRepository : IReservationRepository
{
    private IList<Reservation> _reservations =
    [
        new Reservation { RES_Idx = 1, PAT_Idx = 1, RES_Date = new DateTime(2026, 11, 11), RES_Route = "DSK", RES_Subject = "GNR"},
        new Reservation { RES_Idx = 2, PAT_Idx = 1, RES_Date = new DateTime(2027, 1, 15), RES_Route = "DSK", RES_Subject = "GNR"},
        new Reservation {RES_Idx = 3, PAT_Idx = 3, RES_Date = new DateTime(2026, 2, 18), RES_Route = "ONL", RES_Subject = "SKN"}
    ];

    private int _lastNumber;

    public ReservationRepository()
    {
        _lastNumber = _reservations.Max(x => x.RES_Idx).GetValueOrDefault(0);
    }

    public Reservation? GetReservation(Reservation item)
    {
        return _reservations.FirstOrDefault(x => x.PAT_Idx == item.PAT_Idx && x.RES_Date == item.RES_Date);
    }

    public List<Reservation> GetReservations()
    {
        return [.. _reservations];
    }

    public Reservation SetReservation(Reservation item)
    {
        item.RES_Idx = ++_lastNumber;

        _reservations.Add(item);

        return item;
    }

    public bool ExistsByPatientId(int PAT_Idx)
    {
        return _reservations.Any(x => x.PAT_Idx == PAT_Idx);
    }
}
