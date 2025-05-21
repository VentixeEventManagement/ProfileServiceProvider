using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost]
    public async Task<IActionResult> AddUserInfo(UserRegistrationForm form)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        var result = await _userService.AddUserInfoasync(form);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPost("userId")]
    public async Task<IActionResult> GetUserInfo(string userId)
    {
        if (userId == null)
            return BadRequest();

        var result = await _userService.GetUserInfoAsync(x => x.UserId == userId);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result);
    }


}
