using ExpenseProject.Core;
using ExpenseProject.Core.DTOs;
using ExpenseProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseProject.Controllers
{
        [Authorize]
        [ApiController]
        [Route("[controller]")]
        public class ExpenseController : ControllerBase
        {
            private IExpenseService _expenseService;
            public ExpenseController(IExpenseService expenseService)
            {
                _expenseService = expenseService;
            }

            [HttpGet]
            public IActionResult GetExpenses()
            {
                return Ok(_expenseService.GetExpenses());
            }


            [HttpGet("{id}", Name = "GetSingleExpense")]
            public IActionResult GetSingleExpense(int id)
            {
                return Ok(_expenseService.GetSingleExpense(id));
            }

            [HttpPost]
            public IActionResult CreateExpense(ExpenseDto expense)
            {
                var newExpense = _expenseService.CreateExpense(expense);
                return CreatedAtRoute("GetSingleExpense", new { newExpense.Id }, newExpense);
            }

        [HttpDelete("{id}")]
        public IActionResult DeleteExpense(int id)
        {
            _expenseService.DeleteExpense(id);
            return NoContent(); // Using NoContent to indicate that the operation was successful but there's no content to return.
        }

        [HttpPut("{id}")]
        public IActionResult EditExpense(int id, [FromBody] ExpenseDto expenseDto)
        {
            if (expenseDto == null || expenseDto.Id != id)
            {
                return BadRequest("Expense ID mismatch.");
            }

            var updatedExpense = _expenseService.EditExpense(expenseDto);
            if (updatedExpense == null)
            {
                return NotFound();
            }
            return Ok(updatedExpense);
        }

    }
}