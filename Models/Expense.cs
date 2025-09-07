
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Category { get; set; }
        [Range(0.01, 100000)]
        public decimal Amount { get; set; }
        public string? Note { get; set; }
    }
}