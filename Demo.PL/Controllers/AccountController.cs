using Demo.BLL.Common.Services.EmailSettings;
using Demo.DAL.Entities.Identity;
using Demo.PL.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController/*(UserManager<ApplicationUser> userManager)*/ : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSetting _emailSetting;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSetting emailSetting)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSetting = emailSetting;
        }

        //Register(SignUp), Login, SignOut

        #region Register(SignUp)

        [HttpGet] //Display Form  ==> so i can register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel) //take registerViewModel => has been sent from view
        {
            if (ModelState.IsValid)
            {
                var User = new ApplicationUser()
                {
                    //Badr123@gmail.com  ==> UserName must be unique ==> Badr123
                    UserName = registerViewModel.Email.Split('@')[0],
                    Email = registerViewModel.Email,
                    FName = registerViewModel.FName,
                    LName = registerViewModel.LName,
                    IsAgree = registerViewModel.IsAgree
                };
                //123 ===> M89 (Hashing Function) عشان كدا مكنتش ببعت الباسورد فوق وانا بكريت يوزر من الابلكيشن يوزر
                //Interfaces and Classes [Signture for methods, Methods itself in classes]
                var Result = await _userManager.CreateAsync(User, registerViewModel.Password);  //هستقبله في حاجه عشان اعمل تشيك عليه نجح ولا لا
                if (Result.Succeeded)
                {
                    // Created Successfully and redirect to login page
                    return RedirectToAction("Login");
                }
                else
                {
                    //loop on each error in Result
                    foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(registerViewModel);  //ModelState is not valid or Result not successed
        }

        #endregion

        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)  //View to Controller
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);   //Check user exists with same email or not
                if (user is not null)   //User Exists
                {
                    var flag = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);   //check the return is true or false
                    if (flag) //email exists & password correct
                    {
                        //PasswordSignInAsync => Internally depends on service that generates tokens [encrypted string bvjksjkvwervrkwe651415-1532-5] 
                        var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index"/*ViewOrActionName*/, "Home"/*ControllerName*/);
                    }
                    else  //email exists but password incorrect                   
                        ModelState.AddModelError(string.Empty,"Invalid Login Attempt");
                }
                else
                    ModelState.AddModelError(string.Empty, "Email is not found");
            }
            return View(loginViewModel);
        }

        #endregion

        #region SignOut

        [HttpGet]
        public new async Task<IActionResult> SignOut()   //Masking
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region Forget Password

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel forgetPasswordViewModel)
        {

            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
                if (User is not null)
                {

                    // Generate Url
                    // https://localhost:7091/Account/ResetPasswordUrl?email=doaa@gmail.com
                    // https://localhost:7091/Account/ResetPasswordUrl?email=mariam@gmail.com & token

                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);

                    var resetPassword = Url.Action("ResetPassword", "Account", new
                    {
                        email =
                        forgetPasswordViewModel.Email,
                        token
                    }, Request.Scheme);

                    // Create Email
                    // To, Subject, Body => DomainModel Or Entity (Email) => {To, Subject, Body}

                    var email = new Email()
                    {
                        To = forgetPasswordViewModel.Email,
                        Subject = "Reset Your Password",
                        Body = resetPassword ?? string.Empty
                    };

                    // Send Email
                    _emailSetting.SendEmail(email);


                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Invalid Email");
            }
            return View(forgetPasswordViewModel);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }




        #endregion

        #region Reset Password

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                var user = await _userManager.FindByEmailAsync(email);

                if (user is not null)
                {
                    var Result = await _userManager.ResetPasswordAsync(user, token, resetPasswordViewModel.Password);

                    if (Result.Succeeded)
                        return RedirectToAction(nameof(Login));

                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid Operation Please Try Again");

            return View(resetPasswordViewModel);
        }

        #endregion
    }
}
