using Blog.Models;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Blog.Services.Interfaces;

namespace Blog.Controllers
{
    /// <summary>
    /// Controller for editing users
    /// </summary>
    [Authorize(Roles ="admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IImageService _imageService;

        public UsersController(UserManager<User> userManager, IImageService imageService)
        {
            this._userManager = userManager;
            this._imageService = imageService;
        }
        [HttpGet]
        public IActionResult Index() => View(_userManager.Users.ToList());
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                byte[] avatar = null;
                if (_imageService.IsImage(model.Avatar))
                    avatar = _imageService.GetBytesFrom(model.Avatar);
                    
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    NickName = model.NickName,
                    CreateTime = model.CreateTime,
                    Avatar =   avatar
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Users");
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
                return NotFound();
            ViewBag.Avatar = user.Avatar;
            EditUserViewModel model = new EditUserViewModel 
            { 
                Id = user.Id, 
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NickName = user.NickName,
                CreateTime = user.CreateTime
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.NickName = model.NickName;
                    user.CreateTime = model.CreateTime;

                    
                    if (model.Avatar != null && _imageService.IsImage(model.Avatar))
                        user.Avatar = _imageService.GetBytesFrom(model.Avatar);

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                        return RedirectToAction("Index", "Users");
                    else
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
                await _userManager.DeleteAsync(user);
            return RedirectToAction("Index", "Users");
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            ChangePasswordViewModel model = new ChangePasswordViewModel 
            { 
                Id = user.Id, 
                Email = user.Email
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    IdentityResult result =
                        await passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Index", "Users");
                    }
                    else
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                }
                else
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
            }
            return View(model);
        }
    }
}
