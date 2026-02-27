using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRepository(AppDbContext context) : GenericRepository<User, string>(context), IUserRepository
    {
        public async Task<User?> GetCurrentUserAsync(string email)
        {
            string loweredEmail = email.ToLower();
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == loweredEmail);
        }
    }
}
