using IPLStore.Application.Interfaces;
using IPLStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IPLStore.Infrastructure.Repositories;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    public EfUnitOfWork(AppDbContext db)
    {
        _db = db;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _db.SaveChangesAsync(cancellationToken);
}
