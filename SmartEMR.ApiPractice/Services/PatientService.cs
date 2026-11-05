using SmartEMR.ApiPractice.Entities;

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


    public List<Patient_Res> GetPatients()
    {
        return _patients;
    }


    public Patient_Res? GetPatientById(int id)
    {
        return _patients.FirstOrDefault(x => x.PatientId == id);
    }

    public Patient_Res? SetPatient(Patient_Req request)
    {
        Patient_Res item = new()
        {
            PatientId = _lastNumber + 1,
            Name = request.Name,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            PhoneNumber = request.PhoneNumber
        };

        if (_patients.Any(x => x.PatientId == item.PatientId))
        {
            return null;
        }

        _patients.Add(item);

        return item;
    }
}
