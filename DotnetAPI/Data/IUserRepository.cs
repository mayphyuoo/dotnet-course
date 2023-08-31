using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public IEnumerable<User> GetUsersEf();
        public User GetSingleUserEf(int userId);
        public bool AddEntity<T>(T entityToAdd);
        public bool RemoveEntity<T>(T entityToRemove);
    }
}
