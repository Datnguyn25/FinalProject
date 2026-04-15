using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Services.Email;

namespace FinalProject.Controllers
{
    // Từ khóa 'partial' cho phép chia lớp này thành nhiều file
    public partial class AccountController : Controller
    {
        private readonly WebDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(WebDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
    }
}