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
        private static readonly List<Expense> _expense = new()
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
            return Ok(_expense);
        }

        [HttpPost]
        public ActionResult<Expense> Post([FromBody] Expense expense)
        {
            expense.Id = _expense.Max(e => e.Id) + 1;
            _expense.Add(expense);
            return CreatedAtAction(nameof(Get), new { id = expense.Id }, expense);
        }
    }
}