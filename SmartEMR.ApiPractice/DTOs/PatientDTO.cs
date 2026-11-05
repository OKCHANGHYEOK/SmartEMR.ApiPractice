namespace SmartEMR.ApiPractice.DTOs;

public class PatientDTO
{
    public string Name { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class Patient_Req : PatientDTO
{

}

public class Patient_Res : PatientDTO
{
    public int PatientId { get; set; }
}
