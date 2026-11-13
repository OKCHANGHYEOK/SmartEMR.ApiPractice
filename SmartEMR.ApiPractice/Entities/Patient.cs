namespace SmartEMR.ApiPractice.Entities;

public class Patient
{
    public int? PAT_Idx { get; set; }
    public string? PAT_Name { get; set; }
    public string? PAT_BirthYear { get; set; }
    public string? PAT_BirthMonth { get; set; }
    public string? PAT_BirthDay { get; set; }
    public string? PAT_Sex { get; set; }
    public string? PAT_Hpp1 { get; set; }
    public string? PAT_Hpp2 { get; set; }
    public string? PAT_Hpp3 { get; set; }
    public bool? PAT_IsValid { get; set; } 
}