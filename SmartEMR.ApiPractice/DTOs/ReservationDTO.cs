namespace SmartEMR.ApiPractice.DTOs;

public class ReservationDTO
{
    public int? PAT_Idx { get; set; }
    public DateTime RES_Date { get; set; }
    public string? RES_Route { get; set; }
    public string? RES_Subject { get; set; }
}

public class Reservation_Req : ReservationDTO
{
    
}

public class Reservation_Res : ReservationDTO
{
    public int? RES_Idx { get; set; }
}