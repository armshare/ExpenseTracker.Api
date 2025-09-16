namespace ExpenseTracker.Api.Models.Dto
{
    public class ExpenseCreateDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public Category Category { get; set; }
        public DateTime? Date { get; set; }   // optional: ถ้าไม่ใส่ ให้ backend เติม DateTime.UtcNow
        public string? Note { get; set; }
    }
}