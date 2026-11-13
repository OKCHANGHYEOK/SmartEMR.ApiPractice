using SmartEMR.ApiPractice.Entities;

namespace SmartEMR.ApiPractice.Repository;

public interface IPatientRepository
{
    public Task<List<Patient>> GetPatients(Patient item);
    public Task<Patient?> GetPatientById(int PAT_Idx);
    public Task<Patient?> SetPatient(Patient item);
    public Task<Patient?> UpdatePatient(Patient item);
    public Task<Patient?> DeletePatient(int id);
}
