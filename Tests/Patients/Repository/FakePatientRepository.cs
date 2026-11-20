using SmartEMR.ApiPractice.Entities;
using SmartEMR.ApiPractice.Repository;

namespace Tests.Patients.Repository;

internal sealed class FakePatientRepository : IPatientRepository
{

    private readonly List<Patient> _patients = [
        new Patient { PAT_Idx = 1, PAT_Name = "홍길동", PAT_Sex = "M", PAT_BirthYear = "1999", PAT_BirthMonth = "01", PAT_BirthDay = "11" },
        new Patient { PAT_Idx = 2, PAT_Name = "김철수", PAT_Sex = "M", PAT_BirthYear = "2001", PAT_BirthMonth = "09", PAT_BirthDay = "23"},
        new Patient { PAT_Idx = 3, PAT_Name = "이지현", PAT_Sex = "F", PAT_BirthYear = "2003", PAT_BirthMonth = "04", PAT_BirthDay = "15"}
    ];

    private int _lastNumber;

    public FakePatientRepository()
    {
        _lastNumber = _patients.Max(x => x.PAT_Idx.GetValueOrDefault(0));
    }

    public Task<Patient?> GetPatientById(int PAT_Idx, CancellationToken ct)
    {
        return Task.FromResult(_patients.FirstOrDefault(x => x.PAT_Idx == PAT_Idx));
    }

    public Task<List<Patient>> GetPatients(Patient item, CancellationToken ct)
    {
        return Task.FromResult(_patients);
    }

    public Task<Patient?> SetPatient(Patient item, CancellationToken ct)
    {
        item.PAT_Idx = ++_lastNumber;

        _patients.Add(item);

        return Task.FromResult(_patients.FirstOrDefault(x => x.PAT_Idx == item.PAT_Idx));
    }

    public Task<Patient?> UpdatePatient(Patient item, CancellationToken ct)
    {
        var target = _patients.FirstOrDefault(x => x.PAT_Idx == item.PAT_Idx);
        if (target is not null)
        {
            var props = target.GetType().GetProperties();

            foreach (var prop in props)
            {
                var newValue = prop.GetValue(item);
                prop.SetValue(target, newValue);
            }
        }

        return Task.FromResult(target);
    }


    public Task<Patient?> DeletePatient(int id, CancellationToken ct)
    {
        var target = _patients.FirstOrDefault(x => x.PAT_Idx == id);
        if (target is not null)
        {
            _patients.Remove(target);
        }

        return Task.FromResult(target);
    }

    public int GetLastNumber()
    {
        return _lastNumber;
    }
}
