using ExpenseProject.Core.CustomExceptions;
using ExpenseProject.Core.DTOs;
using ExpenseProject.Core.Utilities;
using ExpenseProject.Data;
using ExpenseProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseProject.Core
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // SignIn method authenticates a user based on username and password.
        public async Task<AuthenticatedUserDto> SignIn(User user)
        {
            // Attempt to find the user in the database by username.
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);
            // Check if user exists and password is correct. If not, throw an exception.
            if (dbUser == null || dbUser.Password == null || _passwordHasher.VerifyHashedPassword(dbUser.Password, user.Password) == Microsoft.AspNet.Identity.PasswordVerificationResult.Failed)
            {
                throw new InvalidUsernamePasswordException("Invalid username or password");
            }
            // If authentication is successful, generate and return a JWT token.
            return new AuthenticatedUserDto()
            {
                Username = user.Username,
                Token = JwtGenerator.GenerateAuthToken(user.Username),
            };
        }

        // SignUp method registers a new user with a hashed password.
        public async Task<AuthenticatedUserDto> SignUp(User user)
        {
            // Check if the username already exists in the database.
            var checkUsername = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.Equals(user.Username));

            if (checkUsername != null)
            {
                throw new UsernameAlreadyExists("Username already exists");
            }

            // Hash the password before saving the user to the database.
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = _passwordHasher.HashPassword(user.Password);
            }

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            // After successful registration, generate and return a JWT token.
            return new AuthenticatedUserDto
            {
                Username = user.Username,
                Token = JwtGenerator.GenerateAuthToken(user.Username)
            };
        }
        // CreateUniqueUsernameFromEmail generates a unique username based on the user's email.
        private string CreateUniqueUsernameFromEmail(string email)
        {
            // Extract the username part of the email.
            var emailSplit = email.Split('@').First();
            var random = new Random();
            var username = emailSplit;

            // Append a random number to the username part of the email until a unique username is found.
            while (_context.Users.Any(u => u.Username.Equals(username)))
            {
                username = emailSplit + random.Next(10000000);
            }

            return username;
        }


    }
}
