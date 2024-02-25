using Chatter.Application.Dtos.Users;

namespace Chatter.Application.Services.Users;

public interface IUserService
{
    /// <summary>
    /// searchValue parametresi username içinde contains atılır
    /// </summary>
    /// <param name="searchValue"></param>
    /// <returns></returns>
    Task<List<UserShortInfoDto>> GetUsersShortInfoAsync(string searchValue);

}