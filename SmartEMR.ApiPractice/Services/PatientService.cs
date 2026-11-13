using Azure.Core;
using SmartEMR.ApiPractice.DTOs;
using SmartEMR.ApiPractice.Entities;
using SmartEMR.ApiPractice.Repository;
using SmartEMR.ApiPractice.Validator;

namespace SmartEMR.ApiPractice.Services;

public partial class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IReservationRepository _reservationRepository;

    public PatientService(IPatientRepository patientRepository, IReservationRepository reservationRepository)
    {
        _patientRepository = patientRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<ServiceResult<Patient_Res>> GetPatients(Patient_Req request)
    {
        var result = new ServiceResult<Patient_Res>();

        var item = new Patient
        {
            PAT_Name = request.PAT_Name,
        };

        var ret = await _patientRepository.GetPatients(item);
        if (ret is null || !ret.Any())
        {
            result.StatusCode = 404;
            result.Message = "환자내역이 존재하지 않습니다.";

            return result;
        }

        result.Items = MapToList(ret);
        result.IsSucess = true;

        return result;
    }

    public async Task<ServiceResult<Patient_Res>> GetPatientById(int id)
    {
        var result = new ServiceResult<Patient_Res>();
        var ret = await _patientRepository.GetPatientById(id);
        
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

    public async Task<ServiceResult<Patient_Res>> SetPatient(Patient_Req request)
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
            PAT_Name = request.PAT_Name,
            PAT_BirthYear = request.PAT_BirthYear,
            PAT_BirthMonth = request.PAT_BirthMonth,
            PAT_BirthDay = request.PAT_BirthDay,
            PAT_Sex = request.PAT_Sex,
            PAT_Hpp1 = request.PAT_Hpp1,
            PAT_Hpp2 = request.PAT_Hpp2,
            PAT_Hpp3 = request.PAT_Hpp3
        };

        var ret = await _patientRepository.SetPatient(item);
        if (ret is null)
        {
            result.Message = "환자 등록 중 오류가 발생했습니다.";
            result.StatusCode = 400;

            return result;
        }

        result.Item = MapToRes(ret);
        result.IsSucess = true;

        return result;
    }

    public async Task<ServiceResult<Patient_Res>> UpdatePatient(int id, Patient_Req request)
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
            PAT_Name = request.PAT_Name,
            PAT_BirthYear = request.PAT_BirthYear,
            PAT_BirthMonth = request.PAT_BirthMonth,
            PAT_BirthDay = request.PAT_BirthDay,
            PAT_Sex = request.PAT_Sex,
            PAT_Hpp1 = request.PAT_Hpp1,
            PAT_Hpp2 = request.PAT_Hpp2,
            PAT_Hpp3 = request.PAT_Hpp3
        };

        var ret = await _patientRepository.UpdatePatient(item);
        if (ret is null)
        {
            result.StatusCode = 400;
            result.Message = "환자저장에 실패했습니다.";

            return result;
        }

        result.Item = MapToRes(ret);
        result.IsSucess = true;

        return result;
    }

    public async Task<ServiceResult<Patient_Res>> DeletePatient(int id)
    {
        var result = new ServiceResult<Patient_Res>();
        var ret = await _patientRepository.DeletePatient(id);
        if (ret is null)
        {
            result.StatusCode = 404;
            result.Message = "해당하는 환자가 존재하지 않습니다.";

            return result;
        }

        //var existsRES = _reservationRepository.ExistsByPatientId(id);
        //if (existsRES)
        //{
        //    result.StatusCode = 409;
        //    result.Message = "예약이 존재하는 환자는 삭제할 수 없습니다.";

        //    return result;
        //}

        result.IsSucess = true;

        return result;
    }
}

// 정적 메서드
public partial class PatientService
{
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
            PAT_BirthYear = patient.PAT_BirthYear,
            PAT_BirthMonth = patient.PAT_BirthMonth,
            PAT_BirthDay = patient.PAT_BirthDay,
            PAT_Sex = patient.PAT_Sex,
            PAT_Hpp1 = patient.PAT_Hpp1,
            PAT_Hpp2 = patient.PAT_Hpp2,
            PAT_Hpp3 = patient.PAT_Hpp3
        };
    }
}