using SmartEMR.ApiPractice.Entities;
using SmartEMR.ApiPractice.Infrastructure;

namespace SmartEMR.ApiPractice.Repository;

public class PatientRepository : BaseRepository, IPatientRepository
{
    //private readonly List<Patient> _patients = [
    //   new Patient {
    //       PAT_Idx = 1,
    //       PAT_Name = "홍길동",
    //       PAT_BirthDate = new DateTime(1999, 7, 14),
    //       PAT_Sex = "M",
    //       PAT_Hpp = "010-1111-1111"
    //   },
    //   new Patient {
    //       PAT_Idx = 2,
    //       PAT_Name = "김철수",
    //       PAT_BirthDate = new DateTime(2003, 11, 29),
    //       PAT_Sex = "M",
    //       PAT_Hpp = "010-2222-2222"
    //   },
    //   new Patient {
    //       PAT_Idx = 3,
    //       PAT_Name = "이영희",
    //       PAT_BirthDate = new DateTime(2001, 4, 9),
    //       PAT_Sex = "F",
    //       PAT_Hpp = "010-3333-3333"
    //   },
    //];

    public PatientRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Patient?> GetPatientById(int PAT_Idx)
    {
        var item = new Patient
        {
            PAT_Idx = PAT_Idx
        };

        var ret = await dbContext.GetItemAsync<Patient>(eSP.proc_Patient_GetPatient, item);
        if (ret is null || !dbContext.retIsSuccess)
        {
            return null;
        }

        return ret;
    }

    public async Task<List<Patient>> GetPatients(Patient item)
    {
        var ret = await dbContext.GetItemsAsync(eSP.proc_Patient_GetPatient, item);
        if (ret is null || !dbContext.retIsSuccess)
        {
            return default!;
        }

        return ret;
    }

    public async Task<Patient?> SetPatient(Patient item)
    {
        var ret = await dbContext.GetItemAsync(eSP.proc_Patient_SetPatient, item);
        if (ret is null || !dbContext.retIsSuccess)
        {
            return null;
        }

        return ret;
    }

    public async Task<Patient?> UpdatePatient(Patient item)
    {
        var ret = await dbContext.GetItemAsync(eSP.proc_Patient_SetPatient, item);
        if (ret is null || !dbContext.retIsSuccess)
        {
            return null;
        }

        return ret;
    }

    public async Task<Patient?> DeletePatient(int PAT_Idx)
    {
        var item = new Patient
        {
            PAT_Idx = PAT_Idx,
            PAT_IsValid = false
        };

        var ret = await dbContext.GetItemAsync(eSP.proc_Patient_SetPatient, item);
        if (ret is null || !dbContext.retIsSuccess)
        {
            return null;
        }

        return ret;
    }
}
