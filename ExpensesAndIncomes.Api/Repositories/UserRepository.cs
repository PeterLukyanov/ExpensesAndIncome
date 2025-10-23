using Models;
using Db;

namespace Repositorys;

public class UserRepository : IUserRepository
{
    private readonly ExpensesAndIncomesDb _db;
    public UserRepository(ExpensesAndIncomesDb db)
    {
        _db = db;
    }
    public IQueryable<User> GetAll()
    {
        return _db.Users.AsQueryable();
    }

    public async Task AddAsync(User user)
    {
        await _db.Users.AddAsync(user);
    }

    public void Remove(User user)
    {
        _db.Users.Remove(user);
    }
}