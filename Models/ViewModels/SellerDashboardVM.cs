namespace FinalProject.ViewModels
{
    public class SellerDashboardVM
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProductsSold { get; set; }

        public List<TopProductVM> TopProducts { get; set; }
    }

    public class TopProductVM
    {
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
    }
}