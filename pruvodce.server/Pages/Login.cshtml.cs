using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace pruvodce.server.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfiguration _configuration;

        public LoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<LoginModel> logger,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToPage("/Dashboard");
                }
                else if (User.IsInRole("Student"))
                {
                    return RedirectToPage("/Students/Index");
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string? username = null;

            var adminPassword = _configuration["LoginPasswords:Admin"];
            var studentPassword = _configuration["LoginPasswords:Student"];

            if (Password == adminPassword)
            {
                username = "admin";
            }
            else if (Password == studentPassword)
            {
                username = "student";
            }
            else
            {
                ErrorMessage = "Nesprávné heslo";
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(
                username,
                Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Uživatel {username} se přihlásil.");

                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToPage("/Dashboard");
                    }
                    else if (roles.Contains("Student"))
                    {
                        return RedirectToPage("/Students/Index");
                    }
                }

                return RedirectToPage("/Dashboard");
            }

            ErrorMessage = "Přihlášení selhalo. Zkuste znovu.";
            return Page();
        }
    }
}