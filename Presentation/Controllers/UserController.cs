using Business.Interfaces;
using Business.Models;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost("add")]
    public async Task<IActionResult> AddUserInfo(UserRegistrationForm form)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        var result = await _userService.AddUserInfoasync(form);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetUserInfo(string userId)
    {
        if (userId == null)
            return BadRequest();

        var result = await _userService.GetUserInfoAsync(x => x.UserId == userId);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateUserInfo(string userId, [FromBody] UserUpdateForm user)
    {
        if (userId == null)
            return BadRequest("User id is missing.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.UpdateProfileInfoAsync(userId, user);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteUserInfo(string userId)
    {
        if (userId == null)
            return BadRequest("User id is missing.");

        var result = await _userService.DeleteProfileInfoAsync(userId);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result);
    }
}
