using System.ComponentModel.DataAnnotations;

namespace ExpenseProject.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        public User User { get; set; }


    }
}
