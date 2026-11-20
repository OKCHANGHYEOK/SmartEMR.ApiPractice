using SmartEMR.ApiPractice.Repository;
using SmartEMR.ApiPractice.Services;
using SmartEMR.ApiPractice.DTOs;
using Tests.Patients.Repository;

namespace Tests.Patients;

public class PatientTest
{
    private readonly FakePatientRepository _fakePatientRepository = new();
    private readonly FakeReservationRepository _fakeReservationRepository = new();
    private readonly PatientService _patientService;

    public PatientTest()
    {
        _patientService = new PatientService(_fakePatientRepository, _fakeReservationRepository);
    }

    [Fact]
    public async Task GetPatients_Success()
    {
        var result = await _patientService.GetPatients(new Patient_Req(), CancellationToken.None);

        Assert.NotNull(result.Items);
        Assert.NotEmpty(result.Items);
        Assert.Equal(3, result.Items.Count);
    }

    [Fact]
    public async Task SetPatient_Success()
    {
        var result = await _patientService.SetPatient(new Patient_Req { PAT_Name = "성하루", PAT_Sex = "F", PAT_BirthYear = "2009", PAT_BirthMonth = "10", PAT_BirthDay = "20" }, CancellationToken.None);

        Assert.True(result.IsSucess);
        Assert.NotNull(result.Item);
        Assert.Equal(_fakePatientRepository.GetLastNumber(), result.Item.PAT_Idx);
    }

    [Fact]
    public async Task SetPatient_FailedByInCorrectInput()
    {
        var result = await _patientService.SetPatient(new Patient_Req { PAT_Name = "김준성", PAT_Sex = "MALE", PAT_BirthYear = "2005", PAT_BirthMonth = "05", PAT_BirthDay = "11" }, CancellationToken.None);

        Assert.False(result.IsSucess);
        Assert.Null(result.Item);
        Assert.Equal(400, result.StatusCode);
    }
}
