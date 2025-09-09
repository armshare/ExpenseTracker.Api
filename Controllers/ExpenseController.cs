using System;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Api.Controllers
{

    [Route("[controller]")]
    public class ExpenseController : ControllerBase
    {

        ILogger<ExpenseController> _logger;

        private readonly ExpenseDbContext _context;

        public ExpenseController(ILogger<ExpenseController> logger, ExpenseDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpense()
        {
            return await _context.Expenses.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetById(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            return expense is not null ? Ok(expense) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Expense>> Post([FromBody] Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Expense updated)
        {
            if (id != updated.Id) return BadRequest();
            _context.Entry(updated).State = EntityState.Modified;
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return NotFound();

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Expense>>> SearchExpense(
            string? category,
            DateTime? startDate,
            DateTime? endDate,
            decimal? minAmount,
            decimal? maxAmount,
            string? sortBy = "date",
            bool desc = false,
            int page = 1,
            int pageSize = 10
        )
        {
            var query = _context.Expenses.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(e => e.Category.Contains(category));

            if (startDate.HasValue)
                query = query.Where(e => e.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(e => e.Date <= endDate.Value);

            if (minAmount.HasValue)
                query = query.Where(e => e.Amount >= minAmount.Value);

            if (maxAmount.HasValue)
                query = query.Where(e => e.Amount <= maxAmount.Value);

            query = sortBy?.ToLower() switch
            {
                "amount" => desc ? query.OrderByDescending(e => e.Amount) : query.OrderBy(e => e.Amount),
                "category" => desc ? query.OrderByDescending(e => e.Category) : query.OrderBy(e => e.Category),
                _ => desc ? query.OrderByDescending(e => e.Date) : query.OrderBy(e => e.Date)
            };

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }
    }
}