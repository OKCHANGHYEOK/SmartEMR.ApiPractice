using System.ComponentModel.DataAnnotations;

namespace SmartEMR.ApiPractice.Entities;

public class Patient
{
    public string Name { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string Gender { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;
}

public class Patient_Req : Patient
{

}

public class Patient_Res : Patient
{
    public int PatientId { get; set; }
}
