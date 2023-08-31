using DotnetAPI.Models;
using System.Linq;

namespace DotnetAPI.Data
{
    public class UserRepository : IUserRepository
    {
        DataContextEF _entityFramework;

        public UserRepository(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public IEnumerable<User> GetUsersEf()
        {
            IEnumerable<User> users = _entityFramework.Users.ToList<User>();

            return users;
        }

        public User GetSingleUserEf(int userId)
        {
            User? user = _entityFramework.Users
                 .Where(u => u.UserId == userId)
                 .FirstOrDefault<User>();

            if (user != null)
            {
                return user;
            }

            throw new Exception("Failed to Get User");
        }

        public bool AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
                return true;
            }
            return false;
        }

        public bool RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
                return true;
            }
            return false;
        }
    }
}
