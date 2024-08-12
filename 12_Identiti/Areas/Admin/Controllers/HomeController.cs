using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _12_Identiti.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles ="Admin")]
[Authorize(policy: "AgePolicy")]
public class HomeController: Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(policy:"adasd")]
    public IActionResult Get()
    {
        return View();
    }
}

