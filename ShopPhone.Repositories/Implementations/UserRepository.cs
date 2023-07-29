using log4net;
using ShopPhone.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {

        private ILog _Logger;
        private readonly ShopPhoneContext _Context;
        public UserRepository(ShopPhoneContext context, ILog logger)
        {
            _Context = context;
            _Logger = logger;
        }

        public async Task<string> AddAsync(User entity)
        {
            try
            {
                await _Context.Set<User>().AddAsync(entity!);
                await _Context.SaveChangesAsync();
                return entity.Login;
            }
            catch (Exception e)
            {
                _Logger.Error(e.Message);
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
                var response = await _Context
                                    .Set<User>()                                    
                                    .FindAsync(id);
                return response;
            }
            catch (Exception e)
            {
                _Logger.Error(e.Message);
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
                var response = await _Context
                                    .Set<User>().ToListAsync();
                                    
                return response;

            }
            catch (Exception e)
            {
                _Logger.Error(e.Message);
                throw;
            }
        }

        public Task UpdateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
