using Business.Factories;
using Business.Models;
using Data.Interfaces;

namespace Business.Services;

public class UserService(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ResponseResult> AddUserInfoasync(UserRegistrationForm form)
    {
        try
        {
            var entity = UserFactory.Create(form);
            if (entity == null)
            {
                return new ResponseResult { Succeeded = false, Message = "Invalid registration form." };
            }

            var result = await _userRepository.AddAsync(entity);
            if (!result)
            {
                return new ResponseResult { Succeeded = false, Message = "Something went wrong with creation.", StatusCode = 400 };
            }

            return new ResponseResult { Succeeded = true, StatusCode = 200, Message = "Profile information was created successfully." };

        } catch (Exception ex)
        {
            return new ResponseResult { Succeeded = false, Message = ex.Message };
        }
    }

    public async Task<ResponseResult<User>> GetUserInfoAsync()
    {

    }
}
