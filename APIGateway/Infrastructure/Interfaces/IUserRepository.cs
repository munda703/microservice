using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository : IGenericRepository<User, string>
    {
        Task<User?> GetCurrentUserAsync(string email);
    }
}
