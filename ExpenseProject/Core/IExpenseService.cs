using ExpenseProject.Core.DTOs;
using ExpenseProject.Models;

namespace ExpenseProject.Core
{
    public interface IExpenseService
    {
        List<ExpenseDto> GetExpenses();
        ExpenseDto GetSingleExpense(int id);
        ExpenseDto CreateExpense(ExpenseDto expense);
        void DeleteExpense(int expense);
        ExpenseDto EditExpense(ExpenseDto expense);

    }
}
