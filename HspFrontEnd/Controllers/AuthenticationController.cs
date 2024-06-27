using HspFrontEnd.Models;
using HspFrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace HspFrontEnd.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterDto register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            var result = await _authenticationService.RegisterAsync(register);

            TempData["msg"] = result.StatusMessage;

            return RedirectToAction(nameof(Register));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var result = await _authenticationService.LoginAsync(login);

            var jwtToken = result.Token;

            if (result.StatusCode == 1 && jwtToken != string.Empty)
            {
                return RedirectToAction("Index", "Home", new { token = jwtToken });
            }
            else
            {
                TempData["msg"] = result.StatusMessage;

                return RedirectToAction(nameof(Login));
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}
