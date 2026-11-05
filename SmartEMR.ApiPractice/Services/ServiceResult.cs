namespace SmartEMR.ApiPractice.Services;

public class ServiceResult<T> where T : class
{
    public T? Item { get; set; }
    public List<T>? Items { get; set; }
    public string? Message { get; set; }
    public bool IsSucess { get; set; } = false;
}
