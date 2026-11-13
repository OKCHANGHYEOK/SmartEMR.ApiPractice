namespace SmartEMR.ApiPractice.Infrastructure;

public interface IAppDbContext
{
    int retCount { get; set; }
    string? retMessage { get; set; }
    bool retIsSuccess { get; set; }

    public Task<T?> GetItemAsync<T>(eSP sp, T item) where T : class, new();
    public Task<List<T>> GetItemsAsync<T>(eSP sp, T item) where T : class, new();
}
