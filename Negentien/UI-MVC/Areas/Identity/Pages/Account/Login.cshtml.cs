using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NT.BL.Domain.users;
using NT.BL.services;

namespace NT.UI.MVC.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly EmailSender _emailSender;

        public LoginModel(SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager, ILogger<LoginModel> logger,
            EmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }
        
        [BindProperty]
        public InputModel Input { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var email = Input.Email;
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(email, Input.Password, 
                    Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Any())
                        {
                            // Determine the redirect URL based on the user's role
                            string redirectUrl;
                            if (roles.Contains("HeadOfPlatform"))
                            {
                                redirectUrl = Url.Content("~/HeadOfPlatform/Oversight");
                            }
                            else if (roles.Contains("Organization"))
                            {
                                redirectUrl = Url.Content("~/Organization/ProjectOversight");
                            }
                            else if (roles.Contains("Attendent"))
                            {
                                redirectUrl = Url.Content("~/Attendent/AttendantFlowOverview");
                            }
                            else
                            {
                                // Redirect to a default page if the user has no specific role
                                redirectUrl = Url.Content("~/");
                            }

                            // Redirect the user to the appropriate page
                            return LocalRedirect(redirectUrl);
                        }
                    }
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    var user = _userManager.FindByEmailAsync(email);
                    if (user.Result != null)
                    {
                        var token = await _userManager.GenerateTwoFactorTokenAsync(user.Result, "Email");
                        await _emailSender.SendEmailFor2FactorAuthentication(token, "2 Factor Authentication", user.Result);
                    }

                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
            return Page();
        }
    }
}
