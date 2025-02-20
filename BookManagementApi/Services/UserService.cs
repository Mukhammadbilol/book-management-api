using BookManagementApi.Domain.Models;

namespace BookManagementApi.Services;

public class UserService : IUserService
{
    private List<User> _users = new List<User>
    {
        new User { Id = 1, Username = "test", Password = "test" }
    };

    public User Authenticate(string username, string password)
    {
        return _users.SingleOrDefault(x => x.Username == username && x.Password == password);
    }
}