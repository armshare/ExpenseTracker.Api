namespace ExpenseTracker.Api.Models.Dto
{
    public class ExpenseCreateDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public Category Category { get; set; }
    }
}