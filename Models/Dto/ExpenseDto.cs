namespace ExpenseTracker.Api.Models.Dto
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public Category Category { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}