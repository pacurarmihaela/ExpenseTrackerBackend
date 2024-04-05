using ExpenseProject.Models;

namespace ExpenseProject.Core.DTOs
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        public static explicit operator ExpenseDto(Expense e) => new ExpenseDto
        {
            Id = e.Id,
            Description = e.Description,
            Amount = e.Amount
        };

    }
}
