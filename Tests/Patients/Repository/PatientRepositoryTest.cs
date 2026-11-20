using SmartEMR.ApiPractice.Entities;

namespace Tests.Patients.Repository;

public class PatientRepositoryTest
{
    [Fact]
    public async Task GetPatientById_WhenPatientExists_ReturnsPatient()
    {
        var repository = new FakePatientRepository();

        var result = await repository.GetPatientById(1, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.PAT_Idx);
        Assert.Equal("홍길동", result.PAT_Name);
    }

    [Fact]
    public async Task GetPatientById_WhenPatientDoesNotExist_ReturnsNull()
    {
        var repository = new FakePatientRepository();

        var result = await repository.GetPatientById(999, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetPatients_ReturnsAllPatients()
    {
        var repository = new FakePatientRepository();

        var result = await repository.GetPatients(new Patient(), CancellationToken.None);

        Assert.Equal(3, result.Count);
        Assert.Contains(result, patient => patient.PAT_Name == "홍길동");
        Assert.Contains(result, patient => patient.PAT_Name == "김철수");
        Assert.Contains(result, patient => patient.PAT_Name == "이지현");
    }

    [Fact]
    public async Task SetPatient_AddsPatientAndAssignsNewId()
    {
        var repository = new FakePatientRepository();
        var patient = new Patient
        {
            PAT_Name = "성하루",
            PAT_Sex = "F"
        };

        var result = await repository.SetPatient(patient, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(4, result.PAT_Idx);
        Assert.Equal("성하루", result.PAT_Name);

        var savedPatient = await repository.GetPatientById(4, CancellationToken.None);
        Assert.Same(result, savedPatient);
    }

    [Fact]
    public async Task UpdatePatient_WhenPatientExists_UpdatesPatient()
    {
        var repository = new FakePatientRepository();
        var patient = new Patient
        {
            PAT_Idx = 1,
            PAT_Name = "수정된 이름",
            PAT_Sex = "F",
            PAT_BirthYear = "2000",
            PAT_BirthMonth = "02",
            PAT_BirthDay = "03"
        };

        var result = await repository.UpdatePatient(patient, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.PAT_Idx);
        Assert.Equal("수정된 이름", result.PAT_Name);
        Assert.Equal("F", result.PAT_Sex);
        Assert.Equal("2000", result.PAT_BirthYear);
    }

    [Fact]
    public async Task UpdatePatient_WhenPatientDoesNotExist_ReturnsNull()
    {
        var repository = new FakePatientRepository();
        var patient = new Patient
        {
            PAT_Idx = 999,
            PAT_Name = "존재하지 않는 환자"
        };

        var result = await repository.UpdatePatient(patient, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeletePatient_WhenPatientExists_RemovesAndReturnsPatient()
    {
        var repository = new FakePatientRepository();

        var result = await repository.DeletePatient(1, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.PAT_Idx);

        var deletedPatient = await repository.GetPatientById(1, CancellationToken.None);
        Assert.Null(deletedPatient);
    }

    [Fact]
    public async Task DeletePatient_WhenPatientDoesNotExist_ReturnsNull()
    {
        var repository = new FakePatientRepository();

        var result = await repository.DeletePatient(999, CancellationToken.None);

        Assert.Null(result);
    }
}
