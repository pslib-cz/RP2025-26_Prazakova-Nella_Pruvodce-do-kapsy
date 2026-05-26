using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pruvodce.server.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
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

            if (Password == "admin123")
            {
                username = "admin";
            }
            else if (Password == "student123")
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