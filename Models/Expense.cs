
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Models
{
    public class Expense
    {
        public int Id { get; set; }

        // ข้อมูลหลักที่ต้องเห็น / ค้นได้
        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        // Enum หรือ string ขึ้นกับที่มึงตั้งไว้ (นี่ใช้ enum Category)
        public Category Category { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        // หมายเหตุ optional
        public string? Note { get; set; }
    }
}