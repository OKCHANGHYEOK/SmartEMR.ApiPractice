namespace SmartEMR.ApiPractice.Entities;

public class Reservation
{
    public int? RES_Idx { get; set; }
    public int? PAT_Idx { get; set; }
    public DateTime RES_Date { get; set; }
    public string? RES_Route { get; set; }
    public string? RES_Subject { get; set; }
}
