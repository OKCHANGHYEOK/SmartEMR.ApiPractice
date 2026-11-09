namespace SmartEMR.ApiPractice.Entities;

public class Patient
{
    public int PAT_Idx { get; set; }
    public string PAT_Name { get; set; } = string.Empty;
    public DateTime? PAT_BirthDate { get; set; }
    public string PAT_BirthYear { get; set; } = string.Empty;
    public string PAT_BirthMonth { get; set; } = string.Empty;
    public string PAT_BirthDay { get; set; } = string.Empty;
    public string PAT_Sex { get; set; } = string.Empty;
    public string PAT_PhoneNum { get; set; } = string.Empty;
    public string PAT_PhoneNum1 { get; set; } = string.Empty;
    public string PAT_PhoneNum2 { get; set; } = string.Empty;
    public string PAT_PhoneNum3 { get; set; } = string.Empty;

}