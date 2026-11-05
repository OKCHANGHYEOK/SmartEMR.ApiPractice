using SmartEMR.ApiPractice.DTOs;
using SmartEMR.ApiPractice.Validator;

namespace SmartEMR.ApiPractice.Services;

public class PatientService : IPatientService
{
    private List<Patient_Res> _patients =
    [
       new Patient_Res { 
           PatientId = 1, 
           Name = "홍길동", 
           BirthDate = new DateTime(1999, 7, 14),  
           Gender = "M",
           PhoneNumber = "010-1111-1111"
       },
       new Patient_Res {
           PatientId = 2,
           Name = "김철수",
           BirthDate = new DateTime(2003, 11, 29),
           Gender = "M",
           PhoneNumber = "010-2222-2222"
       },
       new Patient_Res {
           PatientId = 3,
           Name = "이지현",
           BirthDate = new DateTime(2001, 4, 9),
           Gender = "F",
           PhoneNumber = "010-3333-3333"
       },
    ];

    private int _lastNumber
    {
        get
        {
            return _patients.OrderBy(x => x.PatientId).Last().PatientId;
        }
    }


    public ServiceResult<Patient_Res> GetPatients()
    {
        var result = new ServiceResult<Patient_Res>();

        result.Items = [.. _patients.AsReadOnly()];
        result.IsSucess = true;

        return result;
    }

    public ServiceResult<Patient_Res> GetPatientById(int id)
    {
        var result = new ServiceResult<Patient_Res>();
        var ret = _patients.FirstOrDefault(x => x.PatientId == id);
        
        if (ret is null)
        {
            result.StatusCode = 404;
            result.Message = "해당하는 환자가 존재하지 않습니다.";

            return result;
        }

        result.Item = ret;
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

        var item = new Patient_Res
        {
            PatientId = _lastNumber + 1,
            Name = request.Name,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            PhoneNumber = request.PhoneNumber
        };

        if (_patients.Any(x => x.PatientId == item.PatientId))
        {
            result.StatusCode = 400;
            result.Message = "하나 이상의 중복된 키를 가진 환자가 존재합니다.";

            return result;
        }

        _patients.Add(item);

        result.Item = item;
        result.IsSucess = true;

        return result;
    }
}
