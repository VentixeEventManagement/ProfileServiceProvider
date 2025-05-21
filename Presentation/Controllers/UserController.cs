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
    public async Task<IActionResult> Create(UserRegistrationForm form)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        var result = await _userService.AddUserInfoasync(form);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result);
    }
}
