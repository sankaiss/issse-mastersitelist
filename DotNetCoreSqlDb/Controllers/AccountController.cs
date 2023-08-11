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
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<AccountController> _logger;


    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailService emailService, ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _logger = logger;
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult ForgotPassword()
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
        // Detta kommer att visa alla fel relaterade till lösenordskravet till användaren.
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
    return RedirectToAction("Index", "Home"); // Ersätt med önskad vy och controller
    }


    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
    if (string.IsNullOrEmpty(email))
        return View();  // du kan lägga till ett felmeddelande här om du vill

    var user = await _userManager.FindByEmailAsync(email);
    if (user == null)
        return View();  // av säkerhetsskäl, visa inte ett felmeddelande om e-posten inte finns

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
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("index", "home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }
}
