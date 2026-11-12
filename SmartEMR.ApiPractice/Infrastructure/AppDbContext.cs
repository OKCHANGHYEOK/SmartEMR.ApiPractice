using Microsoft.EntityFrameworkCore;

namespace SmartEMR.ApiPractice.Infrastructure;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public async Task<T?> GetItemAsync<T>(eSP sp, T item)
    {
        return default!;
    }

    public async Task<List<T>> GetItemsAsync<T>(eSP sp, T item)
    {
        return default!;
    }
}
