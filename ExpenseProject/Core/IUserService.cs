using ExpenseProject.Core.DTOs;
using ExpenseProject.Models;

namespace ExpenseProject.Core
{
    public interface IUserService
    {
        Task<AuthenticatedUserDto> SignUp(User user);
        Task<AuthenticatedUserDto> SignIn(User user);
    }
}
