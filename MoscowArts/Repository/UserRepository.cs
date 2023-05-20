using Microsoft.EntityFrameworkCore;
using MoscowArts.DataAccess;
using MoscowArts.Entities;
using MoscowArts.Services;
using System.Linq.Expressions;

namespace MoscowArts.Repository
{
    public class UserRepository : IUserRepository
    { 
    private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id) => await _context.Users.FindAsync(id);
        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync();
        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> exp) => await _context.Users.Where(exp).ToListAsync();

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> RemoveAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> AddRangeAsync(IEnumerable<User> users)
        {
            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();
            return users;
        }

        public async Task<IEnumerable<User>> UpdateRangeAsync(IEnumerable<User> users)
        {
            _context.Users.UpdateRange(users);
            await _context.SaveChangesAsync();
            return users;
        }

        public async Task<IEnumerable<User>> RemoveRangeAsync(IEnumerable<User> users)
        {
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();
            return users;
        }
    }
}
