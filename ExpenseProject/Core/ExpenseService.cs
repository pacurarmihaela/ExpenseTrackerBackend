using ExpenseProject.Core.DTOs;
using ExpenseProject.Data;
using ExpenseProject.Models;
using Microsoft.AspNetCore.Http;

namespace ExpenseProject.Core
{
    public class ExpenseService : IExpenseService
    {
        private readonly AppDbContext _context;
        private readonly User _user;

        // Constructor: Initializes a new instance of the ExpenseService class.
        public ExpenseService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            // Fetches the user from the database based on the username stored in the current HTTP context.
            // Throws an InvalidOperationException if the user is not found, ensuring the service does not operate without a valid user.
            _user = _context.Users
                            .FirstOrDefault(u => u.Username == httpContextAccessor.HttpContext.User.Identity.Name)
                            ?? throw new InvalidOperationException("User not found");
        }

        // Retrieves a single expense by its ID, ensuring it belongs to the current user, and maps it to an ExpenseDto.
        // Throws KeyNotFoundException if the expense is not found, ensuring clear feedback for invalid requests.
        public ExpenseDto GetSingleExpense(int id)
        {
            var expense = _context.Expenses
                                  .Where(e => e.Id == id && e.User.userId == _user.userId)
                                  .Select(e => new ExpenseDto
                                  {
                                      Id = e.Id,
                                      Description = e.Description,
                                      Amount = e.Amount
                                  })
                                  .FirstOrDefault();

            if (expense == null) throw new KeyNotFoundException("Expense not found.");

            return expense;
        }

        // Retrieves all expenses associated with the current user and maps them to ExpenseDto objects.
        public List<ExpenseDto> GetExpenses()
        {
            return _context.Expenses
                           .Where(e => e.User.userId == _user.userId)
                           .Select(e => new ExpenseDto
                           {
                               Id = e.Id,
                               Description = e.Description,
                               Amount = e.Amount
                               // Map other fields as necessary
                           })
                           .ToList();
        }

        // Creates a new expense from an ExpenseDto and associates it with the current user.
        // Returns the ExpenseDto with the ID of the newly created expense.
        public ExpenseDto CreateExpense(ExpenseDto expenseDto)
        {
            var expense = new Expense
            {
                Description = expenseDto.Description,
                Amount = expenseDto.Amount,
                User = _user
            };

            _context.Expenses.Add(expense);
            _context.SaveChanges();

            expenseDto.Id = expense.Id; // Ensuring the DTO reflects the newly created Expense ID
            return expenseDto;
        }

        // Deletes an expense by its ID, ensuring the operation only affects expenses belonging to the current user.
        // Throws KeyNotFoundException if the expense to be deleted is not found.
        public void DeleteExpense(int expenseId)
        {
            var expense = _context.Expenses.FirstOrDefault(e => e.Id == expenseId && e.User.userId == _user.userId);
            if (expense == null) throw new KeyNotFoundException("Expense not found.");

            _context.Expenses.Remove(expense);
            _context.SaveChanges();
        }

        // Updates an existing expense with data from an ExpenseDto, ensuring the expense belongs to the current user.
        // Returns the updated ExpenseDto.
        // Throws KeyNotFoundException if the expense to be updated is not found.
        public ExpenseDto EditExpense(ExpenseDto expenseDto)
        {
            var expense = _context.Expenses.FirstOrDefault(e => e.Id == expenseDto.Id && e.User.userId == _user.userId);
            if (expense == null) throw new KeyNotFoundException("Expense not found.");

            expense.Description = expenseDto.Description;
            expense.Amount = expenseDto.Amount;
           
            _context.SaveChanges();

            return expenseDto; 
        }
    }
}
