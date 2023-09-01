using ShopPhone.DataAccess;
using ShopPhone.Repositories.Interfaces;
using System.Data.Entity;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace ShopPhone.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {

        private ILogger<UserRepository> _logger;
        private readonly ShopPhoneContext _context;
        public UserRepository(ShopPhoneContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<string> AddAsync(User entity)
        {
            try
            {
                await _context.Set<User>().AddAsync(entity!);
                await _context.SaveChangesAsync();
                return entity.Login;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public  async Task<User?> FindAsync(string id)
        {
            try
            {
                var response = await _context
                                    .Set<User>()                                    
                                    .FindAsync(id);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

        public Task<ICollection<User>> FindByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<User>> ListAsync()
        {
            try
            {
                var response = await _context
                                    .Set<User>().ToListAsync();
                                    
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

        public Task UpdateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
