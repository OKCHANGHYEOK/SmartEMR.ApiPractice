using SmartEMR.ApiPractice.Infrastructure;

namespace SmartEMR.ApiPractice.Repository;

public class BaseRepository
{
    protected readonly IAppDbContext dbContext;

    public BaseRepository(IAppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
}
