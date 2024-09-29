using WebApplication1.Entities;

namespace WebApplication1.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByUserNameAsync(string username);
    Task AddUserAsync(User users);
}

public class UserRepository : IUserRepository
{
    private readonly StudentsDBContext _studentsDbContext;
    public UserRepository(StudentsDBContext studentsDbContext)
    {
        _studentsDbContext = studentsDbContext;
    }
    public async Task<User?> GetUserByUserNameAsync(string username)
    {
       return _studentsDbContext!.Users!.FirstOrDefault(x => x.UserName == username);
    }

    public async Task AddUserAsync(User users)
    {
        await _studentsDbContext.Users.AddAsync(users);
        await _studentsDbContext.SaveChangesAsync();

    }
}