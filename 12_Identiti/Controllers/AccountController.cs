using System;
using System.Net;
using System.Net.Mail;
using _12_Identiti.Models.Entities;
using _12_Identiti.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _12_Identiti.Controllers;

public class AccountController :Controller
{
    //UserManager
    //SingManager
    //RoleManager

    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid)
            return View(registerVM);

        AppUser appUser = new AppUser
        {
            UserName = registerVM.Email,
            Email = registerVM.Email,
            City="Baku",
            Picture="default.jpeg"
        };

        var result = await _userManager.CreateAsync(appUser,registerVM.Password);

        if (result.Succeeded)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var confirmLink = Url.Action(action: "ConfirmEmail", controller: "Home", new{email=appUser.Email,token=token},Request.Scheme);

            var smtp = new SmtpClient("smtp.gmail.com",587);
            smtp.Credentials = new NetworkCredential("hesenovfermayil765@gmail.com", "kzku dkxn bapr kmpd");
            smtp.EnableSsl = true;

            MailMessage mail = new MailMessage();
            mail.Subject = "Confirm Your Email";
            mail.Body = $"<a href={confirmLink}>Click to Btn and Confirm Email</a>";
            mail.IsBodyHtml = true;
            mail.From = new MailAddress("hesenovfermayil765@gmail.com");
            mail.To.Add(appUser.Email);

            smtp.Send(mail);

            //await _userManager.AddToRoleAsync(appUser,"User"); rolu istesek burada veririk

            return RedirectToAction("Login");
        }

        else
            foreach(var item in result.Errors)
                ModelState.AddModelError("All",item.Description);

        return View(registerVM);
    }
    //---------------------------------------------------------------------

    public IActionResult AccessDenied()
    {
        return View();
    }







    //---------------------------------------------------------------------
    [HttpGet]
    public IActionResult Login(string? ReturnUrl=null)
    {
        ViewBag.ReturnUrl = ReturnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM,string? ReturnUrl=null)
    {
        if (!ModelState.IsValid)
            return View(loginVM);

        var user = await _userManager.FindByEmailAsync(loginVM.Email);

        if(user is null)
        {
            ModelState.AddModelError("All", "Userrr not found... Ooop...");
            return View(loginVM);
        }
        if(!await _userManager.IsEmailConfirmedAsync(user))
        {
            ModelState.AddModelError("All", "Please confirm your email... Ooop...");
            return View(loginVM);
        }
        //---------------------------------------------------------------------


        //var result = await _userManager.CheckPasswordAsync(user,loginVM.Password);

        //if (result ==false)
        //{
        //    ModelState.AddModelError("All", "Password is wrongg... Ooop...");
        //    return View(loginVM);
        //}

        //await _signInManager.SignInAsync(user,true);

    
        //-----------------------------------------------------------------------

        var result =await _signInManager.PasswordSignInAsync(user,loginVM.Password,true,true);

        if (result.IsLockedOut)
        {
            ModelState.AddModelError("All", $"Your account is locked out => Count: {3-user.AccessFailedCount}");
          
        }
        if (!result.Succeeded)
        {
            ModelState.AddModelError("All", $"Password is wrong");
        }

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(ReturnUrl))
                return Redirect(ReturnUrl);
            //if (Url.IsLocalUrl(ReturnUrl))
            //    return Redirect(ReturnUrl);

            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }
        return View(loginVM);
    }
}

