namespace SmartEMR.ApiPractice.DTOs;

public class PatientDTO
{
    public string PAT_Name { get; set; } = string.Empty;
    public DateTime? PAT_BirthDate { get; set; }
    public string PAT_Sex { get; set; } = string.Empty;
    public string PAT_PhoneNum { get; set; } = string.Empty;
}

public class Patient_Req : PatientDTO
{

}

public class Patient_Res : PatientDTO
{
    public int PAT_Idx { get; set; }
}
