using System;
using System.Linq;
using ExpenseTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Api.Controllers
{

    [Route("[controller]")]
    public class ExpenseController : ControllerBase
    {

        ILogger<ExpenseController> _logger;
        private static readonly List<Expense> _expenses = new()
        {
new Expense{ Id = 1, Date = DateTime.Now.Date, Category = "Food", Amount = 120.50m, Note = "Lunch"},
new Expense{ Id = 2, Date = DateTime.Now.Date, Category = "Transport", Amount = 45.00m, Note = "Bus ticket"}
        };

        public ExpenseController(ILogger<ExpenseController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Expense>> Get()
        {
            return Ok(_expenses);
        }

        [HttpPost]
        public ActionResult<Expense> Post([FromBody] Expense expense)
        {
            expense.Id = _expenses.Max(e => e.Id) + 1;
            _expenses.Add(expense);
            return CreatedAtAction(nameof(Get), new { id = expense.Id }, expense);
        }

        [HttpGet("{id}")]
        public ActionResult<Expense> GetById(int id)
        {
            var expense = _expenses.FirstOrDefault(e => e.Id == id);
            return expense is not null ? Ok(expense) : NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Expense updated)
        {
            var expense = _expenses.FirstOrDefault(e => e.Id == id);
            if (expense == null) return NotFound();
            expense.Date = updated.Date;
            expense.Category = updated.Category;
            expense.Amount = updated.Amount;
            expense.Note = updated.Note;
            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var expense = _expenses.FirstOrDefault(e => e.Id == id);
            if (expense == null) return NotFound();
            _expenses.Remove(expense);
            return NoContent();
        }
    }
}