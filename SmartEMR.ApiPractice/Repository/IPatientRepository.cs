using SmartEMR.ApiPractice.Entities;

namespace SmartEMR.ApiPractice.Repository;

public interface IPatientRepository
{
    public Task<List<Patient>> GetPatients(Patient item, CancellationToken ct);
    public Task<Patient?> GetPatientById(int PAT_Idx, CancellationToken ct);
    public Task<Patient?> SetPatient(Patient item, CancellationToken ct);
    public Task<Patient?> UpdatePatient(Patient item, CancellationToken ct);
    public Task<Patient?> DeletePatient(int id, CancellationToken ct);

    public virtual int GetLastNumber() { return 0; }
}
