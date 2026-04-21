namespace FinalProject.ViewModels
{
    public class CheckoutVM
    {
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string Address { get; set; }
        public string? Note { get; set; }
        public string PaymentMethod { get; set; }
        public Dictionary<int, string> Sizes { get; set; } = new();
    }
}
