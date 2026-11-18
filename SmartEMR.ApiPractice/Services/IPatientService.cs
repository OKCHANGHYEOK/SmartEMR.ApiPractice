using SmartEMR.ApiPractice.DTOs;

namespace SmartEMR.ApiPractice.Services;

public interface IPatientService 
{
    public Task<ServiceResult<Patient_Res>> GetPatients(Patient_Req request, CancellationToken ct);
    public Task<ServiceResult<Patient_Res>> GetPatientById(int id, CancellationToken ct);
    public Task<ServiceResult<Patient_Res>> SetPatient(Patient_Req request, CancellationToken ct);
    public Task<ServiceResult<Patient_Res>> UpdatePatient(int id, Patient_Req request, CancellationToken ct);
    public Task<ServiceResult<Patient_Res>> DeletePatient(int id, CancellationToken ct);
}
