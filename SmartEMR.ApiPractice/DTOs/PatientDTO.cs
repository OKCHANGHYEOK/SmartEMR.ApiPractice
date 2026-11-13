namespace SmartEMR.ApiPractice.DTOs;

public class PatientDTO
{
    public string? PAT_Name { get; set; }
    public string? PAT_BirthYear { get; set; }
    public string? PAT_BirthMonth { get; set; }
    public string? PAT_BirthDay { get; set; }
    public string? PAT_Sex { get; set; }
    public string? PAT_Hpp1 { get; set; }
    public string? PAT_Hpp2 { get; set; }
    public string? PAT_Hpp3 { get; set; }
}

public class Patient_Req : PatientDTO
{

}

public class Patient_Res : PatientDTO
{
    public int? PAT_Idx { get; set; }
}
