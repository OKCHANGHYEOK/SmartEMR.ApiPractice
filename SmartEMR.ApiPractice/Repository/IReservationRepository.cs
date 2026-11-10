namespace SmartEMR.ApiPractice.Repository;

public interface IReservationRepository
{
    bool ExistsByPatientId(int PAT_Idx);
}
