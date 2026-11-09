using SmartEMR.ApiPractice.Entities;

namespace SmartEMR.ApiPractice.Repository;

public class PatientRepository : IPatientRepository
{
    private readonly List<Patient> _patients = [
       new Patient {
           PAT_Idx = 1,
           PAT_Name = "홍길동",
           PAT_BirthDate = new DateTime(1999, 7, 14),
           PAT_Sex = "M",
           PAT_PhoneNum = "010-1111-1111"
       },
       new Patient {
           PAT_Idx = 2,
           PAT_Name = "김철수",
           PAT_BirthDate = new DateTime(2003, 11, 29),
           PAT_Sex = "M",
           PAT_PhoneNum = "010-2222-2222"
       },
       new Patient {
           PAT_Idx = 3,
           PAT_Name = "이지현",
           PAT_BirthDate = new DateTime(2001, 4, 9),
           PAT_Sex = "F",
           PAT_PhoneNum = "010-3333-3333"
       },
    ];

    private int _lastNumber { get; set; }

    public PatientRepository()
    {
        _lastNumber = _patients.Max(x => x.PAT_Idx);
    }

    public Patient? GetPatientById(int PAT_Idx)
    {
        return _patients.FirstOrDefault(x => x.PAT_Idx == PAT_Idx);
    }

    public List<Patient> GetPatients()
    {
        return [.._patients];
    }

    public int GetNextIndexNumber()
    {
        return ++_lastNumber;
    }

    public Patient SetPatient(Patient item)
    {
        _patients.Add(item);
        return item;
    }

    public Patient UpdatePatient(Patient item)
    {
        var target = _patients.FirstOrDefault(x => x.PAT_Idx == item.PAT_Idx);

        target?.PAT_Name = item.PAT_Name;
        target?.PAT_Sex = item.PAT_Sex;
        target?.PAT_BirthDate = item.PAT_BirthDate;
        target?.PAT_PhoneNum = item.PAT_PhoneNum;

        return target ?? default!;
    }
}
