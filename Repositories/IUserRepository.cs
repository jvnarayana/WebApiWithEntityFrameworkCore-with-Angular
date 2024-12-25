using Microsoft.EntityFrameworkCore;
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
        return await _studentsDbContext!.User!.FirstOrDefaultAsync(x => x.UserName == username);
    }

    public async Task AddUserAsync(User users)
    {
        await _studentsDbContext.User.AddAsync(users);
        await _studentsDbContext.SaveChangesAsync();

    }
}