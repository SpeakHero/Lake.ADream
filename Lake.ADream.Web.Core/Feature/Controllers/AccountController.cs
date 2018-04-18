//using Lake.ADream.Infrastructure.Identity;
//using Lake.ADream.Models.Entities.Identity;
//using Lake.ADream.Services;
//using Lake.ADream.ViewModels.AccountViewModels;
//using log4net.Core;
//using log4net.Repository.Hierarchy;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.ComponentModel;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.Extensions.Options;
//using System.Linq.Expressions;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Lake.ADream.EntityFrameworkCore;
//using System.Collections.Generic;
//using Microsoft.Extensions.DependencyInjection;

//namespace Lake.ADream.Web.Core.Controllers
//{
//    [Description("账户管理")]
//    public class AccountController : ADreamControllerBase
//    {
//        private readonly UserManagerService _userManager;
//        private readonly SignInManagerService _signInManager;

//        public AccountController(IServiceCollection services) : base(services)
//        {
//            var options = Options.Create(new IdentityOptions());
          
//        }
//        [TempData]
//        public string ErrorMessage { get; set; }

//        [HttpGet]
//        [AllowAnonymous]
//        [Description("用户登录")]
//        public async Task<IActionResult> Login(string returnUrl = null)
//        {
//            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
//            ViewData["ReturnUrl"] = returnUrl;
//            return View();
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        [Description("用户登录")]
//        [Level(1)]
//        public async Task<IActionResult> Login(LoginViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                LoginoutViewModel loginoutViewModel = new LoginoutViewModel();
//                var result = await _signInManager.PasswordSignInAsync(model.Account, model.Password, model.RememberMe, lockoutOnFailure: true);
//                if (result.Succeeded)
//                {
//                    return JsonOrView(ADreamResult.Success);
//                }
//                if (result.RequiresTwoFactor)
//                {
//                    loginoutViewModel.ReturnUrl = $"{nameof(LoginWith2fa)}?returnUrl={model.ReturnUrl}&rememberMe={model.RememberMe}";
//                    loginoutViewModel.Need2fa = true;
//                    aDreamResult = ADreamResult.Failed(new ADreamError { Description = "需要第二次身份验证" });
//                    aDreamResult.Result = loginoutViewModel;
//                    return JsonOrView(aDreamResult);
//                }
//                if (result.IsLockedOut)
//                {
//                    return IsLockout();
//                }
//                return JsonOrView(ADreamResult.Failed(new ADreamError { Description = "登录失败，请检测你的账户和密码" }));
//            }
//            return ValidFailed();
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        [Description("二次身份验证")]
//        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
//        {
//            // 确保用户首先通过用户名和密码
//            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

//            if (user == null)
//            {
//                throw new ApplicationException($"无法加载双因素身份验证用户。");
//            }

//            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
//            ViewData["ReturnUrl"] = returnUrl;

//            return View(model);
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        [Description("二次身份验证")]
//        [Level(1)]
//        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
//                if (user == null)
//                {
//                    return JsonOrView(ADreamResult.Failed(new ADreamError { Description = "用户不存在，请检测你的账户和密码" }));
//                }
//                else
//                {
//                    var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
//                    var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

//                    if (!result.Succeeded)
//                    {
//                        if (result.IsLockedOut)
//                        {
//                            return IsLockout();
//                        }
//                        return JsonOrView(ADreamResult.Failed(new ADreamError { Description = "验证失败，认证码无效" }));
//                    }
//                    return JsonOrView(ADreamResult.Success);
//                }
//            }
//            return ValidFailed();
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        [Description("使用恢复代码登录")]
//        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
//        {
//            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
//            if (user == null)
//            {
//                throw new ApplicationException($"无法加载双因素身份验证用户。");
//            }

//            ViewData["ReturnUrl"] = returnUrl;

//            return View();
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        [Description("使用恢复代码登录")]
//        [Level(1)]
//        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
//        {
//            if (!ModelState.IsValid)
//            {
//                return ValidFailed();
//            }
//            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
//            if (user == null)
//            {
//                throw new ApplicationException($"无法加载双因素身份验证用户。");
//            }

//            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

//            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

//            if (result.Succeeded)
//            {
//                return JsonOrView(ADreamSignInResult.Success);
//            }
//            if (result.IsLockedOut)
//            {
//                return IsLockout();
//            }
//            else
//            {
//                return JsonOrView(ADreamResult.Failed(new ADreamError { Description = "验证失败，认证码无效" }));
//            }
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        public IActionResult Lockout()
//        {
//            return View();
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        [Description("用户注册")]
//        public IActionResult Register(string returnUrl = null)
//        {
//            ViewData["ReturnUrl"] = returnUrl;
//            return View();
//        }

//        [HttpPost]
//        [Description("用户注册")]
//        [Level(1)]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
//        {
//            ViewData["ReturnUrl"] = returnUrl;
//            if (ModelState.IsValid)
//            {
//                var user = new User { UserName = model.Email, Email = model.Email };
//                var result = await _userManager.CreateAsync(user, model.Password);
//                if (result.Succeeded)
//                {
//                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//                    //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
//                    //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
//                    await _signInManager.SignInAsync(user, isPersistent: false);
//                    return RedirectToLocal(returnUrl);
//                }
//                return Json(result);
//            }
//            return ValidFailed();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Description("退出")]
//        public async Task<IActionResult> Logout()
//        {
//            await _signInManager.SignOutAsync();
//            if (Request.IsAjax())
//            {
//                return Json(true);
//            }
//            return RedirectToAction(nameof(HomeController.Index), "Home");
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        [Description("其他登录方式")]
//        public IActionResult ExternalLogin(string provider, string returnUrl = null)
//        {
//            // Request a redirect to the external login provider.
//            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
//            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
//            return Challenge(properties, provider);
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        [Description("其他登录方式")]
//        [Level(1)]
//        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
//        {
//            if (remoteError != null)
//            {
//                return Json(ADreamResult.Failed(new ADreamError { Description = "远程登录失败" }));
//            }
//            var info = await _signInManager.GetExternalLoginInfoAsync();
//            if (info == null)
//            {
//                return Json(ADreamResult.Failed(new ADreamError { Description = "登录失败" }));
//            }

//            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
//            if (result.Succeeded)
//            {
//                return Json(ADreamResult.Success);
//            }
//            if (result.IsLockedOut)
//            {
//                return IsLockout();
//            }
//            else
//            {
//                ViewData["ReturnUrl"] = returnUrl;
//                ViewData["LoginProvider"] = info.LoginProvider;
//                Claim claim = info.Principal.FindFirst(ClaimTypes.Email);
//                var email = claim?.Value;
//                var errresult = ADreamResult.Failed();
//                errresult.Result = new { ReturnUrl = returnUrl, info.LoginProvider, Email = email };
//                return JsonOrView(errresult);
//            }
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        [Description("其他登录方式确认")]
//        [Level(1)]
//        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
//        {
//            if (ModelState.IsValid)
//            {
//                var info = await _signInManager.GetExternalLoginInfoAsync();
//                if (info == null)
//                {
//                    return JsonOrView(ADreamResult.Failed("在确认期间加载外部登录信息出错。"));
//                }
//                var user = new User { UserName = model.Email, Email = model.Email };
//                var result = await _userManager.CreateAsync(user);
//                if (result.Succeeded)
//                {
//                    result = await _userManager.AddLoginAsync(user, info);
//                    if (result.Succeeded)
//                    {
//                        await _signInManager.SignInAsync(user, isPersistent: false);
//                        return RedirectToLocal(returnUrl);
//                    }
//                }
//                return JsonOrView(result);
//            }

//            return ValidFailed();
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        [Description("确认邮箱")]
//        [Level(1)]
//        public async Task<IActionResult> ConfirmEmail(string userId, string code)
//        {
//            if (userId == null || code == null)
//            {
//                return JsonOrView(ADreamResult.Failed("用户为空或者验证码为空"));
//            }
//            var user = await _userManager.FindByIdAsync(userId);
//            if (user == null)
//            {
//                return JsonOrView(ADreamResult.Failed("用户不存在"));

//            }
//            var result = await _userManager.ConfirmEmailAsync(user, code);
//            return JsonOrView(result);
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        [Description("找回密码")]

//        public IActionResult ForgotPassword()
//        {
//            return View();
//        }

//        [HttpPost]
//        [Description("找回密码")]
//        [Level(1)]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = await _userManager.FindByEmailAsync(model.Email);
//                if (user == null)
//                {
//                    return JsonOrView(ADreamResult.Failed("用户不存在,或者邮箱没有激活"));
//                }
//                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
//                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
//                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
//                //   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
//                return JsonOrView(ADreamResult.Success);
//            }

//            return ValidFailed();
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        [Description("找回密码")]
//        public IActionResult ForgotPasswordConfirmation()
//        {
//            return View();
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        [Description("密码重置")]
//        public IActionResult ResetPassword(string code = null)
//        {
//            if (code == null)
//            {
//                return JsonOrView(ADreamResult.Failed("验证码不能为空"));
//            }
//            var model = new ResetPasswordViewModel { Code = code };
//            return JsonOrView(model);
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        [Description("密码重置")]
//        [Level(1)]
//        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return ValidFailed();
//            }
//            var user = await _userManager.FindByEmailAsync(model.Email);
//            if (user != null)
//            {
//                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
//                if (result.Succeeded)
//                {
//                    return JsonOrView(result);
//                }
//            }
//            return JsonOrView(ADreamResult.Failed("操作失败，notu"));
//        }

//        [HttpGet]
//        [Description("密码重置")]
//        [AllowAnonymous]
//        public IActionResult ResetPasswordConfirmation()
//        {
//            return View();
//        }


//        [HttpGet]
//        ///拒绝访问 401
//        public IActionResult AccessDenied()
//        {
//            return View();
//        }
//        private void AddErrors(ADreamResult result)
//        {
//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError(string.Empty, error.Description);
//            }
//        }
//        private IActionResult IsLockout()
//        {
//            LoginoutViewModel loginoutViewModel = new LoginoutViewModel
//            {
//                ReturnUrl = nameof(Lockout),
//                Lockout = true
//            };
//            var failed = ADreamResult.Failed(new ADreamError { Description = "账户被锁定" });
//            failed.Result = loginoutViewModel;
//            return JsonOrView(failed);
//        }
//        private IActionResult RedirectToLocal(string returnUrl)
//        {
//            if (Url.IsLocalUrl(returnUrl))
//            {
//                if (Request.IsAjax())
//                {
//                    return Json(new ADreamResult { Result = returnUrl });
//                }
//                return Redirect(returnUrl);
//            }
//            else
//            {
//                if (Request.IsAjax())
//                {
//                    return Json(new ADreamResult { Result = $"/Home/{nameof(HomeController.Index)}" });
//                }
//                return RedirectToAction(nameof(HomeController.Index), "Home");
//            }
//        }

//    }
//}