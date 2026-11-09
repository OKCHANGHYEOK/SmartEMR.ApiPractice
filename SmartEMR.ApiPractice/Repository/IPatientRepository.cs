using SmartEMR.ApiPractice.DTOs;
using SmartEMR.ApiPractice.Entities;

namespace SmartEMR.ApiPractice.Repository;

public interface IPatientRepository
{
    public List<Patient> GetPatients();
    public Patient? GetPatientById(int PAT_Idx);
    public int GetNextIndexNumber();
    public Patient SetPatient(Patient item);
}
