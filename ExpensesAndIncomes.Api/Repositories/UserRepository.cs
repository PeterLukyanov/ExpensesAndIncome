using Models;
using Db;

namespace Repositorys;

public class UserRepository : IUserRepository
{
    private readonly ExpensesAndIncomesDb db;
    public UserRepository(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }
    public IQueryable<User> GetAll()
    {
        return db.Users.AsQueryable();
    }

    public async Task AddAsync(User user)
    {
        await db.Users.AddAsync(user);
    }

    public void Remove(User user)
    {
        db.Users.Remove(user);
    }
}