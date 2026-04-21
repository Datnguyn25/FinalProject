namespace FinalProject.ViewModels
{
    public class UserProfileVM
    {
        public string FullName { get; set; }
        public string Email { get; set; } // Thường chỉ để xem, không cho sửa nếu là ID
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string AvatarUrl { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public IFormFile AvatarFile { get; set; } // Để upload ảnh
    }
}
