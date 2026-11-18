using SmartEMR.ApiPractice.Entities;
using SmartEMR.ApiPractice.Infrastructure;

namespace SmartEMR.ApiPractice.Repository;

public class PatientRepository : BaseRepository, IPatientRepository
{
    public PatientRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Patient?> GetPatientById(int PAT_Idx, CancellationToken ct)
    {
        var item = new Patient
        {
            PAT_Idx = PAT_Idx
        };

        var ret = await dbContext.GetItemAsync<Patient>(eSP.proc_Patient_GetPatient, item, ct);
        if (ret is null || !dbContext.retIsSuccess)
        {
            return null;
        }

        return ret;
    }

    public async Task<List<Patient>> GetPatients(Patient item, CancellationToken ct)
    {
        var ret = await dbContext.GetItemsAsync(eSP.proc_Patient_GetPatient, item, ct);

        if (ret is null || !dbContext.retIsSuccess)
        {
            return default!;
        }

        return ret;
    }

    public async Task<Patient?> SetPatient(Patient item, CancellationToken ct)
    {
        var ret = await dbContext.GetItemAsync(eSP.proc_Patient_SetPatient, item, ct);
        if (ret is null || !dbContext.retIsSuccess)
        {
            return null;
        }

        return ret;
    }

    public async Task<Patient?> UpdatePatient(Patient item, CancellationToken ct)
    {
        var ret = await dbContext.GetItemAsync(eSP.proc_Patient_SetPatient, item, ct);
        if (ret is null || !dbContext.retIsSuccess)
        {
            return null;
        }

        return ret;
    }

    public async Task<Patient?> DeletePatient(int PAT_Idx, CancellationToken ct)
    {
        var item = new Patient
        {
            PAT_Idx = PAT_Idx,
            PAT_IsValid = false
        };

        var ret = await dbContext.GetItemAsync(eSP.proc_Patient_SetPatient, item, ct);
        if (ret is null || !dbContext.retIsSuccess)
        {
            return null;
        }

        return ret;
    }
}
