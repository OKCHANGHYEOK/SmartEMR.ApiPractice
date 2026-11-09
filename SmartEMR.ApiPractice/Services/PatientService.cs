using SmartEMR.ApiPractice.DTOs;
using SmartEMR.ApiPractice.Entities;
using SmartEMR.ApiPractice.Repository;
using SmartEMR.ApiPractice.Validator;

namespace SmartEMR.ApiPractice.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public ServiceResult<Patient_Res> GetPatients()
    {
        var result = new ServiceResult<Patient_Res>();
        var ret = _patientRepository.GetPatients();

        result.Items = MapToList(ret);
        result.IsSucess = true;

        return result;
    }

    public ServiceResult<Patient_Res> GetPatientById(int id)
    {
        var result = new ServiceResult<Patient_Res>();
        var ret = _patientRepository.GetPatientById(id);
        
        if (ret is null)
        {
            result.StatusCode = 404;
            result.Message = "해당하는 환자가 존재하지 않습니다.";

            return result;
        }

        result.Item = MapToRes(ret);
        result.IsSucess = true;

        return result;
    }

    public ServiceResult<Patient_Res> SetPatient(Patient_Req request)
    {
        var result = new ServiceResult<Patient_Res>();
        var validate = PatientValidator.Validate(request);

        if (!validate.IsSuccess)
        {
            result.StatusCode = 400;
            result.Message = validate.Message;

            return result;
        }

        var item = new Patient
        {
            PAT_Idx = _patientRepository.GetNextIndexNumber(),
            PAT_Name = request.PAT_Name,
            PAT_BirthDate = request.PAT_BirthDate,
            PAT_Sex = request.PAT_Sex,
            PAT_PhoneNum = request.PAT_PhoneNum
        };

        var ret = _patientRepository.SetPatient(item);
        if (ret is null)
        {
            result.Message = "환자 저장 중 오류가 발생했습니다.";
            result.StatusCode = 400;

            return result;
        }

        result.Item = MapToRes(ret);
        result.IsSucess = true;

        return result;
    }

    private static List<Patient_Res> MapToList(List<Patient> patients)
    {
        var result = new List<Patient_Res>();

        foreach (var patient in patients)
        {
            result.Add(MapToRes(patient));
        }

        return result;
    }

    private static Patient_Res MapToRes(Patient patient)
    {
        return new Patient_Res
        {
            PAT_Idx = patient.PAT_Idx,
            PAT_Name = patient.PAT_Name,
            PAT_BirthDate = patient.PAT_BirthDate,
            PAT_Sex = patient.PAT_Sex,
            PAT_PhoneNum  = patient.PAT_PhoneNum
        };
    }
}
