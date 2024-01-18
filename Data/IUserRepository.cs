using StoredProcuduresTest.Dtos;
using StoredProcuduresTest.Models;

namespace StoredProcuduresTest.Data
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public Task AddEntity(UserDto entity);
        public  Task<bool> UpdateEntity(User entity);
        public  Task<bool> DeleteEntity(int id);
        public Task<User> GetSingle(int id);
        public Task<IEnumerable<User>> Get();
        
    }
}
