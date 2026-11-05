using SmartEMR.ApiPractice.Entities;

namespace SmartEMR.ApiPractice.Services;

public interface IPatientService 
{
    public List<Patient_Res> GetPatients();
    public Patient_Res? GetPatientById(int id);
    public Patient_Res? SetPatient(Patient_Req request);
}
