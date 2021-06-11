using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using spgnn.Models;
using spgnn.ViewModels;

namespace spgnn.Controllers
{
    [Route("user/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [ActionName("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new User() {UserName = model.UserName, Email = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View();
        }
            
        [ActionName("StartLogin")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ActionName("EndLogin")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var first = _userManager.Users.FirstOrDefault();
                var users = _userManager.Users.Count();
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    await _signInManager.SignInAsync(user, false);
                }
                else
                {
                    ModelState.AddModelError("usernotfound", "Пользователь не найден");
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("loginpassworderror", "Не указан логин/пароль");
            }

            return View();
        }

        [ActionName("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = new EditUserViewModel {Id = user.Id, UserName = user.UserName,Email = user.Email };
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if(user!=null)
                {
                    
                    
                    if (model.OldPassword != null || model.Password != null || model.ConfirmPassword != null)
                    {
                        var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
                        if (!changePasswordResult.Succeeded)
                        {
                            foreach (var error in changePasswordResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }

                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded && ModelState.ErrorCount == 0)
                    {
                        await _signInManager.SignOutAsync();
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }
        
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}