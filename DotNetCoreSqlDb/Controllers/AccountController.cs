using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DotNetCoreSqlDb.Models;
using DotNetCoreSqlDb.Services;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreSqlDb.Controllers;
using Microsoft.Extensions.Logging;



public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<AccountController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;


    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService, ILogger<AccountController> logger, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _logger = logger;
        _roleManager = roleManager;
    }

public IActionResult Register()
{
    // Hämta lösenordskraven från UserManager
    var passwordPolicy = _userManager.Options.Password;

   
    ViewBag.PasswordLengthRequirement = passwordPolicy.RequiredLength;
    ViewBag.RequireDigit = passwordPolicy.RequireDigit;
    ViewBag.RequireUppercase = passwordPolicy.RequireUppercase;
    ViewBag.RequireLowercase = passwordPolicy.RequireLowercase;
    ViewBag.RequireNonAlphanumeric = passwordPolicy.RequireNonAlphanumeric;

    return View();
}

    public IActionResult ForgotPassword()
    {
        return View();
    }
    public IActionResult AccessDenied()
    {
        return View();
    }
    public IActionResult ForgotPasswordConfirmation()
    {
    return View();
    }

    public IActionResult PasswordResetSuccess()
    {
    return View();
    }


    public IActionResult ResetPassword(string userId, string token)
    {
    return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
    if (!ModelState.IsValid)
    {
        return View(model);
    }

    var user = await _userManager.FindByIdAsync(model.UserId);
    if (user == null)
    {
        return View("Error"); 
    }

    var resetPasswordResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
    if (resetPasswordResult.Succeeded)
    {        
        try 
        {
            return RedirectToAction("PasswordResetSuccess", "Account");
        }
        catch (Exception)
        {
            return View("Error");
        }
    }

    foreach (var error in resetPasswordResult.Errors)
    {
       
        ModelState.AddModelError(string.Empty, error.Description);
    }
    return View(model);
    }


    [HttpGet]
    public IActionResult Login()
    {
        if (_signInManager.IsSignedIn(User))
        {
            return View("AlreadyLoggedIn");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
    if (ModelState.IsValid)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            // Inloggning lyckades
            return RedirectToAction("Index", "Sites"); 
        }
        ModelState.AddModelError(string.Empty, "Inloggning misslyckades");
    }

    return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
    await _signInManager.SignOutAsync();
    return RedirectToAction("Login", "Account"); 
    }


    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
    if (string.IsNullOrEmpty(email))
        return View();  

    var user = await _userManager.FindByEmailAsync(email);
    if (user == null)
        return View();  

    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token }, protocol: HttpContext.Request.Scheme);

    await _emailService.SendEmailAsync(email, "Reset Password", $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>");

    return RedirectToAction("ForgotPasswordConfirmation");
    }

    [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser 
                { 
                    UserName = model.Email, 
                    Email = model.Email,
                    FullName = model.FullName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Efter framgångsrik skapande av användare, tilldela "User" rollen till den nya användaren
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");

                    if (!addToRoleResult.Succeeded)
                    {
                        foreach (var error in addToRoleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Denna kod kommer att köra om ModelState är ogiltig ELLER om _userManager.CreateAsync misslyckas.
            return View(model);
        }

}
