using _12_Identiti.Datas;
using _12_Identiti.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using _12_Identiti.Services.Policies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});



builder.Services.AddIdentity<AppUser, IdentityRole>(option =>
{
    option.User.RequireUniqueEmail = true;
    option.Password.RequireDigit = false;
    option.Password.RequiredLength = 6;


    //Lockout
    option.Lockout.MaxFailedAccessAttempts = 3;
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMilliseconds(5000);
    option.Lockout.AllowedForNewUsers = true;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMilliseconds(10000);

});


//CustomValidator
builder.Services.AddTransient<IPasswordValidator<AppUser>, _12_Identiti.Validators.PasswordValidator>();

//Add Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AgePolicy", policy =>
        policy.Requirements.Add(new AgeRequirement(21)));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
       name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}"
   );

//using var container = app.Services.CreateScope();
//var userManager = container.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
//var roleManager= container.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//var adminRole = await roleManager.RoleExistsAsync("Admin");
//if (!adminRole)
//    await roleManager.CreateAsync(new IdentityRole { Name="Admin"});

//var adminUser = await userManager.FindByNameAsync("Cavid");

//if(adminUser is null)
//{
//    var result = await userManager.CreateAsync(new AppUser
//    {
//        UserName = "Cavid",
//        Email = "cavid@gmail.com",
//        EmailConfirmed=true,
//        City="Baku",
//        Picture="default.png"
//    },"Admin12#");

//    if (result.Succeeded)
//    {
//        var user = await userManager.FindByNameAsync("Cavid");
//        await userManager.AddToRoleAsync(user,"Admin");
//    }
//}

app.Run();
