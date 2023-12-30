using DotNetAPI.Models;

namespace DotNetAPI.Data {

    public interface IUserRepository {
        public bool SaveChanges();
        public void Add<T>(T entityToAdd);
        public void Remove<T>(T entityToRemove);
        public IEnumerable<User> GetUsers();
        public User GetSingleUser(int userId);
        public UserSalary GetSingleUserSalary(int userId);
        public UserJobInfo GetSingleUserJobInfo(int userId);

    }
}