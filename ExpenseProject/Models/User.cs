using System.ComponentModel.DataAnnotations;

namespace ExpenseProject.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

    }
}
