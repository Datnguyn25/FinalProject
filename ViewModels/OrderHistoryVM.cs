namespace FinalProject.ViewModels
{
    public class OrderHistoryVM
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProductName { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; } // Pending, Processing, Completed, Cancelled
        public string PaymentStatus { get; set; }
    }
}