using BookManagementApi.Domain.Models;

namespace BookManagementApi.Services;

public interface IUserService
{
    User Authenticate(string username, string password);
}
