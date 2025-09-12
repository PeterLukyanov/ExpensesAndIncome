using Models;

namespace Repositorys;

public interface IUserRepository
{
    Task AddAsync(User user);
    void Remove(User user);
    IQueryable<User> GetAll();
}