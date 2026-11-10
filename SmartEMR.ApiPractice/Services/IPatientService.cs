using SmartEMR.ApiPractice.DTOs;

namespace SmartEMR.ApiPractice.Services;

public interface IPatientService 
{
    public ServiceResult<Patient_Res> GetPatients();
    public ServiceResult<Patient_Res> GetPatientById(int id);
    public ServiceResult<Patient_Res> SetPatient(Patient_Req request);
    public ServiceResult<Patient_Res> UpdatePatient(int id, Patient_Req request);
    public ServiceResult<Patient_Res> DeletePatient(int id);
}
