using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Documentation;
using Presentation.Extensions.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.Controllers;

[UseApiKey]
[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [Consumes("multipart/form-data")]
    [HttpPost("add")]
    [SwaggerOperation(Summary = "Adds profile information.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Profile information was created successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Profile request contained invalid properties or missing properties.")]
    [SwaggerRequestExample(typeof(UserRegistrationForm), typeof(UserRegistrationForm_Example))]
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
    [SwaggerOperation(Summary = "Retrieving profile information.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Profile information was retrieved successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Missing user id.")]
    public async Task<IActionResult> GetUserInfo([FromQuery] string userId)
    {
        if (userId == null)
            return BadRequest();

        var result = await _userService.GetUserInfoAsync(x => x.UserId == userId);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllUsersInfo()
    {
        var result = await _userService.GetAllProfilesAsync();
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [Consumes("multipart/form-data")]
    [HttpPost("update")]
    [SwaggerOperation(Summary = "Updating profile information.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User information updated successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Missing user id or new profile information contained invalid properties or missing properties.")]
    [SwaggerRequestExample(typeof(UserUpdateForm), typeof(UserUpdateForm_Example))]
    public async Task<IActionResult> UpdateUserInfo([FromForm] UserUpdateForm user)
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.UpdateProfileInfoAsync(user.UserId, user);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPost("delete")]
    [SwaggerOperation(Summary = "Deleting profile information.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User information deleted successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Missing user id.")]
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
