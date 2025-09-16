using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.Api.Models;
using ExpenseTracker.Api.Models.Dto;
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
        private readonly IMapper _mapper;
        public ExpenseController(ILogger<ExpenseController> logger, ExpenseDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAll()
        {
            var expenses = await _context.Expenses.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ExpenseDto>>(expenses));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetById(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            return expense is not null ? Ok(expense) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> Create([FromBody] ExpenseCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var expense = _mapper.Map<Expense>(dto);
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ExpenseDto>(expense);
            return CreatedAtAction(nameof(GetAll), new { id = expense.Id }, result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Expense updated)
        {
            if (id != updated.Id) return BadRequest("Expense ID mismatch");

            var existingExpense = await _context.Expenses.FindAsync(id);
            if (existingExpense == null)
                return NotFound();

            existingExpense.Date = updated.Date;
            existingExpense.Category = updated.Category;
            existingExpense.Amount = updated.Amount;

            await _context.SaveChangesAsync();

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
        public async Task<ActionResult<PageResult<ExpenseDto>>> SearchExpense(
     string? category,
     DateTime? startDate,
     DateTime? endDate,
     decimal? minAmount,
     decimal? maxAmount,
     string sortBy = "date",
     bool desc = false,
     int page = 1,
     int pageSize = 10
 )
        {
            var query = _context.Expenses.AsQueryable();

            // Filter
            if (!string.IsNullOrEmpty(category))
                query = query.Where(e => e.Category.ToString().Contains(category));

            if (startDate.HasValue)
                query = query.Where(e => e.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(e => e.Date <= endDate.Value);

            if (minAmount.HasValue)
                query = query.Where(e => e.Amount >= minAmount.Value);

            if (maxAmount.HasValue)
                query = query.Where(e => e.Amount <= maxAmount.Value);

            // Sort
            query = sortBy.ToLower() switch
            {
                "amount" => desc ? query.OrderByDescending(e => e.Amount) : query.OrderBy(e => e.Amount),
                "category" => desc ? query.OrderByDescending(e => e.Category) : query.OrderBy(e => e.Category),
                _ => desc ? query.OrderByDescending(e => e.Date) : query.OrderBy(e => e.Date),
            };

            // Paging
            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var result = new PageResult<ExpenseDto>
            {
                Items = _mapper.Map<IEnumerable<ExpenseDto>>(items),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return Ok(result);
        }
    }
}