namespace SmartEMR.ApiPractice.Infrastructure;

public interface IAppDbContext
{
    public Task<T?> GetItemAsync<T>(eSP sp, T item);
    public Task<List<T>> GetItemsAsync<T>(eSP sp, T item);
}
