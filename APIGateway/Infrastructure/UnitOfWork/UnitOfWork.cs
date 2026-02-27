using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork(AppDbContext context, IUserRepository userRepository) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;
        public IUserRepository Users { get; } = userRepository;
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose() => _context.Dispose();
    }
}
